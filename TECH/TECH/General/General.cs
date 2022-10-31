using System;
using System.Collections.Generic;
using System.Text;

namespace TECH.General
{
   public class General
    {
        public enum StaffStatus
        {
            Active = 1, // đang làm việc
            InActive = 2 // nghỉ làm việc
        }
        public enum OrdersStatus
        {
            Delivered = 1, // đã giao hàng
            Cancel = 2 // Trả lại hàng
        }
        public enum ProductStatus
        {
            Show = 1, // Sản phẩm hót
            Hide = 2, // Trả lại hàng
            Wait = 3
        }
    }
}
