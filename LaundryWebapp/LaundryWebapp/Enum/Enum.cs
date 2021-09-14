using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaundryWebapp.Enum
{
    public static class Enum
    {
        public enum Status
        {
            Process,
            Done,
            Complete
        }

        public enum PaymentStatus
        {
            Paid,
            NotPaid
        }
    }
}