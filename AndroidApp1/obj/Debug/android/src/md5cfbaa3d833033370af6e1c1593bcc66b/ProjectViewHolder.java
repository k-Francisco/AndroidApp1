package md5cfbaa3d833033370af6e1c1593bcc66b;


public class ProjectViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("AndroidApp1.Adapters.ProjectViewHolder, AndroidApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ProjectViewHolder.class, __md_methods);
	}


	public ProjectViewHolder (android.view.View p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == ProjectViewHolder.class)
			mono.android.TypeManager.Activate ("AndroidApp1.Adapters.ProjectViewHolder, AndroidApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
