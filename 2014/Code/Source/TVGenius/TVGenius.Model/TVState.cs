namespace TVGenius.Model
{
    /// <summary>
    /// 电视机的状态
    /// </summary>
    public enum TVState
    {
        /// <summary>
        /// 离线
        /// </summary>
        [MarkAttribute(Mark = "离线")]
        Offline,

        /// <summary>
        /// 工作中
        /// </summary>
        [MarkAttribute(Mark = "工作中")]
        PowerOn,

        /// <summary>
        /// 休眠
        /// </summary>
        [MarkAttribute(Mark = "休眠")]
        Hibernate
    }
}
