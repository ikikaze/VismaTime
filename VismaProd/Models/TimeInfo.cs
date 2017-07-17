namespace VismaProd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("TimeInfo")]
    public partial class TimeInfo
    {
        public Guid Id { get; set; }

        public Guid UID { get; set; }

        public Guid PID { get; set; }

         public DateTime startTime { get; set; }

        public int hours { get; set; }

        public DateTime endTime { get; set; }

        public virtual FreeLancer FreeLancer { get; set; }

        public virtual Project Project { get; set; }
    }
}
