using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

public class GPGEditor : MonoBehaviour
{
    [MenuItem("Nerdiacs/UpdateGPGFiles")]
    public static void GenerateManifest()
    {
        // Give an error if the appid is null or contains a character
        if (NerdGPG.appID == "" || Regex.Matches(NerdGPG.appID, @"[a-zA-Z]").Count > 0) {
            UnityEngine.Debug.LogError("Invalid GPG App ID. Please recheck your GPG App ID in NerdGPG.cs.");
            return;
        }

        string manifestOut = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
        string stringsOut = Path.Combine(Application.dataPath, "Plugins/Android/res/values/nerdiacs.xml");

         if (!File.Exists(stringsOut)) {
             UnityEngine.Debug.LogError("Couldnt find nerdiacs.xml at: " + stringsOut + "! Somethings wrong, please check if you have installed the NerdGPG Package correctly.");
             return;
         }

        // only copy over a fresh copy of the AndroidManifest if one does not exist
        if (!File.Exists(manifestOut)) {
            string manifestIn = Path.Combine(EditorApplication.applicationContentsPath, "PlaybackEngines/androidplayer/AndroidManifest.xml");
            File.Copy(manifestIn, manifestOut);
        }

        UpdateManifest(manifestOut);

        UpdateStrings(stringsOut);

        UnityEngine.Debug.Log("Succesfully updated files.");
    }

    private static XmlNode FindChildNode(XmlNode parent, string name)
    {
        XmlNode curr = parent.FirstChild;
        while (curr != null) {
            if (curr.Name.Equals(name)) {
                return curr;
            }
            curr = curr.NextSibling;
        }
        return null;
    }

    private static XmlElement FindMainActivityNode(XmlNode parent)
    {
        XmlNode curr = parent.FirstChild;
        while (curr != null) {
            if (curr.Name.Equals("activity") && curr.FirstChild != null && curr.FirstChild.Name.Equals("intent-filter")) {
                return curr as XmlElement;
            }
            curr = curr.NextSibling;
        }
        return null;
    }

    private static XmlElement FindElementWithAndroidName(string name, string androidName, string ns, string value, XmlNode parent)
    {
        var curr = parent.FirstChild;
        while (curr != null) {
            if (curr.Name.Equals(name) && curr is XmlElement && ((XmlElement)curr).GetAttribute(androidName, ns) == value) {
                return curr as XmlElement;
            }
            curr = curr.NextSibling;
        }
        return null;
    }

     public static void UpdateStrings(string fullPath)
     {
         XmlDocument doc = new XmlDocument();
         doc.Load(fullPath);

         XmlNode manNode = FindChildNode(doc, "resources");
         XmlElement appIdNode = null;

         var curr = manNode.FirstChild;
         while (curr != null) {
             if (curr.Name.Equals("string") && curr is XmlElement && ((XmlElement)curr).GetAttribute("name") == "gpg_app_id") {
                 appIdNode = curr as XmlElement;
                 break;
             }
             curr = curr.NextSibling;
         }

         if (appIdNode == null) {
             appIdNode = doc.CreateElement("string");
             appIdNode.SetAttribute("name", "gpg_app_id");
             appIdNode.InnerText = NerdGPG.appID;
             manNode.AppendChild(appIdNode);
         } else {
             appIdNode.InnerText = NerdGPG.appID;
         }

         doc.Save(fullPath);
     }

    public static void UpdateManifest(string fullPath)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(fullPath);

        XmlNode manNode = FindChildNode(doc, "manifest");
        XmlNode dict = FindChildNode(manNode, "application");

        string ns = dict.GetNamespaceOfPrefix("android");

		XmlElement gpgMeta = FindElementWithAndroidName("meta-data", "name", ns, "com.google.android.gms.games.APP_ID", dict);
        if (gpgMeta == null) {
            gpgMeta = doc.CreateElement("meta-data");
            gpgMeta.SetAttribute("name", ns, "com.google.android.gms.games.APP_ID");
            gpgMeta.SetAttribute("value", ns, "@string/gpg_app_id");
            dict.AppendChild(gpgMeta);
        }

		gpgMeta = FindElementWithAndroidName("meta-data", "name", ns, "com.google.android.gms.appstate.APP_ID", dict);
		if (gpgMeta == null)
		{
			gpgMeta = doc.CreateElement("meta-data");
			gpgMeta.SetAttribute("name",  ns, "com.google.android.gms.appstate.APP_ID");
            gpgMeta.SetAttribute("value", ns, "@string/gpg_app_id");
			dict.AppendChild(gpgMeta);
		}

        gpgMeta = FindElementWithAndroidName("activity", "name", ns, "com.nerdiacs.nerdgpgplugin.DummyActivity", dict);
		if (gpgMeta == null)
		{
			gpgMeta = doc.CreateElement("activity");
			gpgMeta.SetAttribute("name",  ns, "com.nerdiacs.nerdgpgplugin.DummyActivity");
			gpgMeta.SetAttribute("label", ns, "@string/app_name");
			dict.AppendChild(gpgMeta);        
		}		
		
		XmlElement mainActivity = FindMainActivityNode(dict);
		var mainActivityName = mainActivity.GetAttribute("name", ns);
		if (mainActivityName == "com.facebook.unity.FBUnityPlayerActivity" )
		{
			mainActivity.SetAttribute("name", ns, "com.nerdiacs.nerdgpgplugin.NerdUnityPlayerActivity");
		}
        
        doc.Save(fullPath);
    }
}
