import type {ReactNode} from 'react';
import {useCallback, useEffect, useRef} from 'react';
import {resolveInitialLegacyNavigation} from '../legacyRedirects.mjs';

const pagefindScriptId = 'humanizer-pagefind-components';
const pagefindStylesheetId = 'humanizer-pagefind-styles';
let pagefindLoad: Promise<void> | undefined;

type Props = {
  children: ReactNode;
};

type PagefindModalElement = HTMLElement & {
  open(): void;
};

function loadPagefind(): Promise<void> {
  pagefindLoad ??= Promise.all([
    new Promise<void>((resolve, reject) => {
      const existing = document.getElementById(
        pagefindStylesheetId,
      ) as HTMLLinkElement | null;
      if (existing?.sheet) {
        resolve();
        return;
      }

      const link = existing ?? document.createElement('link');
      link.addEventListener('load', () => resolve(), {once: true});
      link.addEventListener('error', () => reject(new Error('Pagefind CSS')), {
        once: true,
      });
      if (!existing) {
        link.id = pagefindStylesheetId;
        link.rel = 'stylesheet';
        link.href = '/pagefind/pagefind-component-ui.css';
        document.head.appendChild(link);
      }
    }),
    new Promise<void>((resolve, reject) => {
      const existing = document.getElementById(
        pagefindScriptId,
      ) as HTMLScriptElement | null;
      if (customElements.get('pagefind-modal')) {
        resolve();
        return;
      }

      const script = existing ?? document.createElement('script');
      script.addEventListener('load', () => resolve(), {once: true});
      script.addEventListener('error', () => reject(new Error('Pagefind JS')), {
        once: true,
      });
      if (!existing) {
        script.id = pagefindScriptId;
        script.type = 'module';
        script.src = '/pagefind/pagefind-component-ui.js';
        document.head.appendChild(script);
      }
    }),
    customElements.whenDefined('pagefind-modal'),
  ]).then(() => undefined);
  return pagefindLoad;
}

async function getReadyPagefindModal(): Promise<PagefindModalElement> {
  const deadline = performance.now() + 5000;
  let readyDialog: HTMLDialogElement | null = null;
  let readySince = 0;
  while (performance.now() < deadline) {
    const modal =
      document.querySelector<PagefindModalElement>('pagefind-modal');
    const dialog = modal?.querySelector('dialog') ?? null;
    if (modal && dialog?.querySelector('input[type="search"]')) {
      if (dialog !== readyDialog) {
        readyDialog = dialog;
        readySince = performance.now();
      } else if (performance.now() - readySince >= 100) {
        return modal;
      }
    }
    await new Promise((resolve) => window.setTimeout(resolve, 25));
  }

  throw new Error('Pagefind modal did not initialize.');
}

export default function Root({children}: Props): ReactNode {
  const trigger = useRef<HTMLButtonElement>(null);
  const openAllVersions = useCallback(async () => {
    trigger.current?.setAttribute('aria-busy', 'true');
    if (trigger.current) trigger.current.textContent = 'Loading…';
    try {
      await loadPagefind();
      const modal = await getReadyPagefindModal();
      const dialog = modal.querySelector('dialog');
      if (!dialog) throw new Error('Pagefind dialog did not initialize.');

      trigger.current?.setAttribute('aria-controls', dialog.id);
      trigger.current?.setAttribute('aria-expanded', 'true');
      dialog.addEventListener(
        'close',
        () => {
          trigger.current?.setAttribute('aria-expanded', 'false');
          trigger.current?.focus();
        },
        {once: true},
      );
      modal.open();
    } catch (error) {
      pagefindLoad = undefined;
      document.getElementById(pagefindScriptId)?.remove();
      document.getElementById(pagefindStylesheetId)?.remove();
      console.error(error);
    } finally {
      trigger.current?.setAttribute('aria-busy', 'false');
      if (trigger.current) trigger.current.textContent = 'All versions';
    }
  }, []);

  useEffect(() => {
    const destination = resolveInitialLegacyNavigation({
      hash: window.location.hash,
      origin: window.location.origin,
      pathname: window.location.pathname,
      referrer: document.referrer,
      search: window.location.search,
    });
    if (destination) {
      window.location.replace(destination);
      return;
    }

    const handleShortcut = (event: KeyboardEvent) => {
      const active = document.activeElement;
      if (
        event.key.toLowerCase() !== 'k' ||
        !event.shiftKey ||
        (!event.metaKey && !event.ctrlKey) ||
        event.altKey ||
        active instanceof HTMLInputElement ||
        active instanceof HTMLTextAreaElement ||
        (active instanceof HTMLElement && active.isContentEditable)
      ) {
        return;
      }

      event.preventDefault();
      void openAllVersions();
    };
    document.addEventListener('keydown', handleShortcut);
    return () => document.removeEventListener('keydown', handleShortcut);
  }, [openAllVersions]);

  return (
    <>
      {children}
      <span className="humanizerAllVersionsShell">
        <button
          aria-busy="false"
          aria-expanded="false"
          aria-haspopup="dialog"
          aria-keyshortcuts="Meta+Shift+K Control+Shift+K"
          className="humanizerAllVersionsTrigger"
          onClick={() => void openAllVersions()}
          ref={trigger}
          type="button">
          All versions
        </button>
      </span>
      <span
        dangerouslySetInnerHTML={{
          __html: '<pagefind-modal reset-on-close></pagefind-modal>',
        }}
      />
    </>
  );
}
