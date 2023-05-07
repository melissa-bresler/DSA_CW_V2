using System;


namespace Tube_Walking_Guide
{
    internal class LinkedList
    {

        protected ListNode head = null;   // points to the head of the list
        protected int length = 0;      // number of nodes in the list

        public LinkedList()
        {
            head = null;   // empty list
            length = 0;      // no nodes in the list
        }

        public bool isEmpty()
        {
            return (length == 0);       // or ( head == null )
        }

        public void insertAtHead(Object item)
        {
            ListNode newItem = new ListNode(item, head);  // .next ) ;

            head = newItem;

            length++;
        }
        private ListNode findItem(Object item)
        {
            if (!isEmpty())
            {
                ListNode current = new ListNode();

                current = head;

                while ((current != null) && (!(item.Equals(current.Value))))
                {
                    current = current.Next;
                }
                return current;
            }
            else
            {
                return null;
            }
        }

        public bool insertAfter(Object newItem, Object afterItem)
        {
            ListNode afterNode = findItem(afterItem);

            if (afterNode != null)
            {
                ListNode newItemNode = new ListNode(newItem, afterNode.Next);
                afterNode.Next = newItemNode;

                length++;

                return true;
            }
            else
            {
                return false;
            }
        }

        public Object deleteHead()
        {
            if (!isEmpty())
            {
                Object headItem = head.Value;

                head = head.Next;
                return headItem;
            }
            else
            {
                return null;
            }
        }

        public void insertAtTail(Object value)
        {
            ListNode newTail = new ListNode(value);

            if (isEmpty())
            {
                head = newTail;
            }
            else
            {
                ListNode current = new ListNode();
                current = head;

                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newTail;
            }
        }
    }
}

