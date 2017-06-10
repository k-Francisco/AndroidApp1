package md5ea1ff66e5cf45835067fc5339d2f537d;


public class LoginWebViewClient
	extends android.webkit.WebViewClient
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_shouldOverrideUrlLoading:(Landroid/webkit/WebView;Ljava/lang/String;)Z:GetShouldOverrideUrlLoading_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"n_onPageStarted:(Landroid/webkit/WebView;Ljava/lang/String;Landroid/graphics/Bitmap;)V:GetOnPageStarted_Landroid_webkit_WebView_Ljava_lang_String_Landroid_graphics_Bitmap_Handler\n" +
			"";
		mono.android.Runtime.register ("AndroidApp1.Helpers.LoginWebViewClient, AndroidApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LoginWebViewClient.class, __md_methods);
	}


	public LoginWebViewClient () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LoginWebViewClient.class)
			mono.android.TypeManager.Activate ("AndroidApp1.Helpers.LoginWebViewClient, AndroidApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public LoginWebViewClient (md53efdd954dced941f1cccdd6377b3a9c5.Splashscreen p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == LoginWebViewClient.class)
			mono.android.TypeManager.Activate ("AndroidApp1.Helpers.LoginWebViewClient, AndroidApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "AndroidApp1.Activities.Splashscreen, AndroidApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public boolean shouldOverrideUrlLoading (android.webkit.WebView p0, java.lang.String p1)
	{
		return n_shouldOverrideUrlLoading (p0, p1);
	}

	private native boolean n_shouldOverrideUrlLoading (android.webkit.WebView p0, java.lang.String p1);


	public void onPageStarted (android.webkit.WebView p0, java.lang.String p1, android.graphics.Bitmap p2)
	{
		n_onPageStarted (p0, p1, p2);
	}

	private native void n_onPageStarted (android.webkit.WebView p0, java.lang.String p1, android.graphics.Bitmap p2);

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
