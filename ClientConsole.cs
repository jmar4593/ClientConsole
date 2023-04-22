using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Unity.VisualScripting;
using System;
//This script gets attached to the Main Camera 

namespace DisplayControl
{
    public class ClientConsole : MonoBehaviour
    {
        //Add my data members
        #region 
        private GameObject canvas;
        private GameObject scrollView;
        private GameObject viewPort;
        private GameObject scrollbarHorizontal;
        private GameObject scrollbarVertical;
        private GameObject content;
        private GameObject hSlidingArea;
        private GameObject vSlidingArea;
        private GameObject hHandle;
        private GameObject vHandle;
        private GameObject logLine;
        AsyncOperationHandle<Sprite> backgroundOperation;
        private Sprite backGround;
        #endregion
        // Start is called before the first frame update 
        void Start()
        {
            makeClientDisplay();
            StartCoroutine(readyMyImages());

        }
        private void makeClientDisplay()
        {
            //Create all GameObjects 
            #region 

            //Create UICanvas
            canvas = new GameObject("Canvas", typeof(RectTransform), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas.layer = 5;
            canvas.transform.SetParent(transform, false);
            //Create UICanvas>Scroll View 
            scrollView = new GameObject("Scroll View", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(ScrollRect));
            scrollView.layer = 5;
            scrollView.transform.SetParent(canvas.transform, false);
            //Create UICanvas>Scroll View>Viewport 
            viewPort = new GameObject("Viewport", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Mask));
            viewPort.layer = 5;
            viewPort.transform.SetParent(scrollView.transform, false);
            //Create UICanvas>Scroll View>Viewport>Scrollbar Horizontal 
            scrollbarHorizontal = new GameObject("Scrollbar Horizontal", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Scrollbar));
            scrollbarHorizontal.layer = 5;
            scrollbarHorizontal.transform.SetParent(scrollView.transform, false);
            //Create UICanvas>Scroll View>Viewport>Scrollbar Vertical 
            scrollbarVertical = new GameObject("Scrollbar Vertical", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Scrollbar));
            scrollbarVertical.layer = 5;
            scrollbarVertical.transform.SetParent(scrollView.transform, false);
            //Create UICanvas>Scroll View>Viewport>Content 
            content = new GameObject("Content", typeof(RectTransform), typeof(ContentSizeFitter), typeof(VerticalLayoutGroup));
            content.layer = 5;
            content.transform.SetParent(viewPort.transform, false);
            //Create UICanvas>Scroll View>Viewport>Scrollbar Horizontal>Sliding Area 
            hSlidingArea = new GameObject("Sliding Area", typeof(RectTransform));
            hSlidingArea.layer = 5;
            hSlidingArea.transform.SetParent(scrollbarHorizontal.transform, false);
            //Create UICanvas>Scroll View>Viewport>Scrollbar Vertical>Sliding Area 
            vSlidingArea = new GameObject("Sliding Area", typeof(RectTransform));
            vSlidingArea.layer = 5;
            vSlidingArea.transform.SetParent(scrollbarVertical.transform, false);
            //Create UICanvas>Scroll View>Viewport>Scrollbar Horizontal>Sliding Area>Handle 
            hHandle = new GameObject("Handle", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            hHandle.layer = 5;
            hHandle.transform.SetParent(hSlidingArea.transform, false);
            //Create UICanvas>Scroll View>Viewport>Scrollbar Vertical>Sliding Area>Handle 
            vHandle = new GameObject("Handle", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            vHandle.layer = 5;
            vHandle.transform.SetParent(vSlidingArea.transform, false);
            #endregion
            //Setup all Gameobjects 
            #region 
            //Setup UICanvas 
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            //Setup UICanvas>Scroll View 
            scrollView.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            scrollView.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            scrollView.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            scrollView.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            Vector2 scrollCanvasSize = scrollView.transform.parent.GetComponent<RectTransform>().rect.size;
            scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollCanvasSize.x, scrollCanvasSize.y / 5);
            Vector2 aScrollPos = scrollView.GetComponent<RectTransform>().anchoredPosition;
            Vector2 aScrollSiz = scrollView.GetComponent<RectTransform>().sizeDelta;
            int scrollBorderOffset = 30;
            scrollView.GetComponent<RectTransform>().anchoredPosition = new Vector2(aScrollPos.x + scrollBorderOffset, aScrollPos.y + scrollBorderOffset);
            scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(aScrollSiz.x - scrollBorderOffset * 2, aScrollSiz.y);
            scrollView.GetComponent<ScrollRect>().scrollSensitivity = 25f;
            scrollView.GetComponent<ScrollRect>().content = content.GetComponent<RectTransform>();
            scrollView.GetComponent<ScrollRect>().viewport = viewPort.GetComponent<RectTransform>();
            scrollView.GetComponent<ScrollRect>().horizontalScrollbar = scrollbarHorizontal.GetComponent<Scrollbar>();
            scrollView.GetComponent<ScrollRect>().horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollView.GetComponent<ScrollRect>().horizontalScrollbarSpacing = -3;
            scrollView.GetComponent<ScrollRect>().verticalScrollbar = scrollbarVertical.GetComponent<Scrollbar>();
            scrollView.GetComponent<ScrollRect>().verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollView.GetComponent<ScrollRect>().verticalScrollbarSpacing = -3;
            //Setup UICanvas>Scroll View>Viewport 
            viewPort.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            viewPort.GetComponent<Image>().type = Image.Type.Sliced;
            viewPort.GetComponent<Mask>().showMaskGraphic = false;
            //Setup UICanvas>Scroll View>Viewport>Scrollbar Horizontal 
            scrollbarHorizontal.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            scrollbarHorizontal.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
            scrollbarHorizontal.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            scrollbarHorizontal.GetComponent<RectTransform>().sizeDelta = new Vector2(-97, 20);
            scrollbarHorizontal.GetComponent<Scrollbar>().targetGraphic = hHandle.GetComponent<Image>();
            scrollbarHorizontal.GetComponent<Scrollbar>().handleRect = hHandle.GetComponent<RectTransform>();
            //Setup UICanvas>Scroll View>Viewport>Scrollbar Vertical 
            scrollbarVertical.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
            scrollbarVertical.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            scrollbarVertical.GetComponent<RectTransform>().pivot = new Vector2(1, 1);
            scrollbarVertical.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 0);
            //scrollbarvert stuff
            scrollbarVertical.GetComponent<Scrollbar>().targetGraphic = vHandle.GetComponent<Image>();
            scrollbarVertical.GetComponent<Scrollbar>().handleRect = vHandle.GetComponent<RectTransform>();
            scrollbarVertical.GetComponent<Scrollbar>().direction = Scrollbar.Direction.BottomToTop;
            //Setup UICanvas>Scroll View>Viewport>Content 
            content.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            content.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            content.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 300);
            content.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            content.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            //Setup UICanvas>Scroll View>Viewport>Scrollbar Horizontal>Sliding Area 
            hSlidingArea.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            hSlidingArea.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            hSlidingArea.GetComponent<RectTransform>().sizeDelta = new Vector2(-20, -20);
            //Setup UICanvas>Scroll View>Viewport>Scrollbar Vertical>Sliding Area 
            vSlidingArea.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            vSlidingArea.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            vSlidingArea.GetComponent<RectTransform>().sizeDelta = new Vector2(-20, -20);
            //Setup UICanvas>Scroll View>Viewport>Scrollbar Horizontal>Sliding Area>Handle 
            hHandle.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            //Setup UICanvas>Scroll View>Viewport>Scrollbar Vertical>Sliding Area>Handle 
            vHandle.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            #endregion
        }
        private IEnumerator readyMyImages()
        {
            backgroundOperation = Addressables.LoadAssetAsync<Sprite>("Assets/Images/Background.psd");
            yield return backgroundOperation;
            scrollView.GetComponent<Image>().sprite = backgroundOperation.Result;
            scrollView.GetComponent<Image>().type = Image.Type.Sliced;
            scrollView.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
            scrollbarHorizontal.GetComponent<Image>().sprite = backgroundOperation.Result;
            scrollbarHorizontal.GetComponent<Image>().type = Image.Type.Sliced;
            scrollbarVertical.GetComponent<Image>().sprite = backgroundOperation.Result;
            scrollbarVertical.GetComponent<Image>().type = Image.Type.Sliced;
            backgroundOperation = Addressables.LoadAssetAsync<Sprite>("Assets/Images/UISprite.psd");
            yield return backgroundOperation;
            hHandle.GetComponent<Image>().sprite = backgroundOperation.Result;
            hHandle.GetComponent<Image>().type = Image.Type.Sliced;
            vHandle.GetComponent<Image>().sprite = backgroundOperation.Result;
            vHandle.GetComponent<Image>().type = Image.Type.Sliced;
        }
        public void callToClientDisplay(string ToClientMessage, Transform scrollviewContent)
        {
            //Create Gameobject 
            logLine = new GameObject("Log Line", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            logLine.layer = 5;
            logLine.transform.SetParent(scrollviewContent, false);
            //Setup Gameobject 
            logLine.GetComponent<TextMeshProUGUI>().fontSize = 12;
            logLine.GetComponent<TextMeshProUGUI>().text = $"{ToClientMessage} ({DateTime.Now.ToString()})";
        }


    }
}