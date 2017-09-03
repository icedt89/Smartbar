namespace JanHafner.Smartbar.Extensibility
{
    /// <summary>
    /// Defines finer behavior during an application import.
    /// </summary>
    public enum ApplicationCreateTargetBehavior
    {
        /// <summary>
        /// If there is already an application on the target position, the target gets deleted and replaced with the source application.
        /// </summary>
        OverrideTarget = 0,

        /// <summary>
        /// If there is already an application on the target position, the operation is skipped.
        /// </summary>
        IgnoreTarget = 1
    }
}
