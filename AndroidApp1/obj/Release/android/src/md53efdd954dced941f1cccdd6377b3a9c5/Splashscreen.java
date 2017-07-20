package md53efdd954dced941f1cccdd6377b3a9c5;


public class Splashscreen
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("AndroidApp1.Activities.Splashscreen, AndroidApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Splashscreen.class, __md_methods);
	}


	public Splashscreen () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Splashscreen.class)
			mono.android.TypeManager.Activate ("AndroidApp1.Activities.Splashscreen, AndroidApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
