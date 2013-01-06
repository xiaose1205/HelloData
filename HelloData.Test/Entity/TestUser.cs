using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloData.FrameWork.Data;

namespace HelloData.Test.Entity
{
   public class TestUser : BaseEntity
    {
        public TestUser()
        {
            base.SetIni(this, "TestUser");
        }

        public string   firstName  { get; set; }

        public static class Columns
        {
            public const string id = "id";
            public const string firstName = "firstName";
        }
    }
}
