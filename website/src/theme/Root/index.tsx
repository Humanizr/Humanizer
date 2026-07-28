import type {ReactNode} from 'react';
import {useEffect} from 'react';
import {resolveInitialLegacyNavigation} from '../legacyRedirects.mjs';

const pagefindScriptId = 'humanizer-pagefind-components';

type Props = {
  children: ReactNode;
};

export default function Root({children}: Props): ReactNode {
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

    if (!document.getElementById(pagefindScriptId)) {
      const script = document.createElement('script');
      script.id = pagefindScriptId;
      script.type = 'module';
      script.src = '/pagefind/pagefind-component-ui.js';
      document.head.appendChild(script);
    }
  }, []);

  return (
    <>
      {children}
      <span
        className="humanizerAllVersionsShell"
        dangerouslySetInnerHTML={{
          __html:
            '<pagefind-modal-trigger placeholder="All versions" shortcut="mod+shift+k"></pagefind-modal-trigger>',
        }}
      />
      <span
        dangerouslySetInnerHTML={{
          __html: '<pagefind-modal reset-on-close></pagefind-modal>',
        }}
      />
    </>
  );
}
