namespace JanHafner.Smartbar.ProcessApplication
{
    internal enum NativeProcessStartErrorCode
    {
        /// <summary>
        /// The operation was cancelled by the user.
        /// Should be catched on Process.Start()!
        /// </summary>
        Cancelled = 1223
    }
}
