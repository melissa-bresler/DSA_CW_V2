using System;


namespace Tube_Walking_Guide
{
    internal class ListNode
    {

        public Object Value { get; set; }
        public ListNode Next { get; set; }

        public ListNode()
        {
            Value = null;
            Next = null;
        }

        public ListNode(Object item)
        {
            this.Value = item;
            this.Next = null;
        }

        public ListNode(Object item, ListNode next)
        {
            this.Value = item;
            this.Next = next;
        }
    }
}
