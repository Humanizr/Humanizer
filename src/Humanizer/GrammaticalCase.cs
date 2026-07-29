namespace Humanizer;

/// <summary>
/// Options for specifying the desired grammatical case for the output words
/// </summary>
public enum GrammaticalCase
{
    /// <summary>
    /// Indicates the subject of a finite verb
    /// </summary>
    Nominative,
    /// <summary>
    /// Indicates the possessor of another noun
    /// </summary>
    Genitive,
    /// <summary>
    /// Indicates the indirect object of a verb
    /// </summary>
    Dative,
    /// <summary>
    /// Indicates the direct object of a verb
    /// </summary>
    Accusative,
    /// <summary>
    /// Indicates an object used in performing an action
    /// </summary>
    Instrumental,
    /// <summary>
    /// Indicates the object of a preposition
    /// </summary>
    Prepositional,
    /// <summary>
    /// Indicates motion away from a noun
    /// </summary>
    Ablative,
    /// <summary>
    /// Indicates accompaniment
    /// </summary>
    Comitative,
    /// <summary>
    /// Indicates the agent of a transitive verb in an ergative construction
    /// </summary>
    Ergative,
    /// <summary>
    /// Indicates location
    /// </summary>
    Locative,
    /// <summary>
    /// Indicates a form used before a case-marking postposition or suffix
    /// </summary>
    Oblique,
    /// <summary>
    /// Indicates a partial or indefinite quantity
    /// </summary>
    Partitive,
    /// <summary>
    /// Indicates direct address
    /// </summary>
    Vocative,
    /// <summary>
    /// Indicates motion out of or away from within
    /// </summary>
    Elative,
    /// <summary>
    /// Indicates motion into
    /// </summary>
    Illative,
    /// <summary>
    /// Indicates association or accompaniment expressed by the Malayalam sociative case
    /// </summary>
    Sociative,
    /// <summary>
    /// Indicates an endpoint or limit
    /// </summary>
    Terminative,
    /// <summary>
    /// Indicates transition into a state
    /// </summary>
    Translative,
    /// <summary>
    /// Indicates the unmarked argument of an intransitive verb or object of a transitive verb
    /// </summary>
    Absolutive,
    /// <summary>
    /// Indicates the Estonian short illative form
    /// </summary>
    Additive,
    /// <summary>
    /// Indicates location within
    /// </summary>
    Inessive,
    /// <summary>
    /// Indicates motion onto or toward
    /// </summary>
    Allative,
    /// <summary>
    /// Indicates location on or at
    /// </summary>
    Adessive,
    /// <summary>
    /// Indicates a temporary state or role
    /// </summary>
    Essive,
    /// <summary>
    /// Indicates absence or being without
    /// </summary>
    Abessive,
    /// <summary>
    /// Indicates comparison or equivalence
    /// </summary>
    Equative,
    /// <summary>
    /// Indicates motion or direction toward
    /// </summary>
    Directive,
    /// <summary>
    /// Indicates direction toward a destination
    /// </summary>
    Lative,
    /// <summary>
    /// Indicates an intended beneficiary
    /// </summary>
    Benefactive,
    /// <summary>
    /// Indicates a cause or reason
    /// </summary>
    Causal,
}