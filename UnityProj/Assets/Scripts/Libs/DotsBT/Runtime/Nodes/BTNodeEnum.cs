namespace DotsBT
{
    public enum EMathOperation
    {
        LessThan,
        LessThanOrEqualTo,
        EqualTo,
        NotEqualTo,
        GreaterThanOrEqualTo,
        GreaterThan,
        BetweenAndExcludeBoth,
        BetweenAndIncludeLeft,
        BetweenAndIncludeRight,
        BetweenAndIncludeBoth,
    }
        
    public enum EAgentState
    {
        None,
        Request,
        Moving,
    }
}