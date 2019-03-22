using System;

namespace  Cream
{
	
	public class Trail
	{
		private System.Collections.ArrayList trail = new System.Collections.ArrayList();
		
		public virtual int Size()
		{
			return trail.Count;
		}
		
		public virtual void  Push(Variable v)
		{
			var pair = new Object[]{v, v.Domain};
			trail.Add(pair);
		}
		
		public virtual void  Undo(int size0)
		{
			for (int size = trail.Count; size > size0; size--)
			{
				var pair = (Object[]) SupportClass.StackSupport.Pop(trail);
				var v = (Variable) pair[0];
				v.Domain = (Domain) pair[1];
			}
		}
	}
}