﻿namespace Assets.Scripts.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class CUIHttpImageScript : CUIComponent
    {
        public float m_cachedTextureValidDays = 2f;
        public bool m_cacheTexture = true;
        public bool m_forceSetImageUrl;
        private enHttpImageState m_httpImageState;
        private Image m_image;
        private ImageAlphaTexLayout m_imageDefaultAlphaTexLayout;
        private Sprite m_imageDefaultSprite;
        public string m_imageUrl;
        public GameObject m_loadingCover;
        public bool m_setNativeSize;
        private static CCachedTextureManager s_cachedTextureManager;

        [DebuggerHidden]
        private IEnumerator DownloadImage(string url)
        {
            <DownloadImage>c__Iterator37 iterator = new <DownloadImage>c__Iterator37();
            iterator.url = url;
            iterator.<$>url = url;
            iterator.<>f__this = this;
            return iterator;
        }

        public override void Initialize(CUIFormScript formScript)
        {
            if (!base.m_isInitialized)
            {
                base.Initialize(formScript);
                this.m_image = base.get_gameObject().GetComponent<Image>();
                this.m_imageDefaultSprite = this.m_image.get_sprite();
                if (this.m_image is Image2)
                {
                    this.m_imageDefaultAlphaTexLayout = (this.m_image as Image2).alphaTexLayout;
                }
                if (this.m_cacheTexture && (s_cachedTextureManager == null))
                {
                    s_cachedTextureManager = new CCachedTextureManager();
                }
                this.m_httpImageState = enHttpImageState.Unload;
                if (this.m_loadingCover != null)
                {
                    this.m_loadingCover.CustomSetActive(true);
                }
                if (base.get_gameObject().get_activeInHierarchy() && !string.IsNullOrEmpty(this.m_imageUrl))
                {
                    this.LoadTexture(this.m_imageUrl);
                }
            }
        }

        private void LoadTexture(string url)
        {
            if (this.m_httpImageState != enHttpImageState.Loaded)
            {
                if (this.m_cacheTexture)
                {
                    Texture2D cachedTexture = s_cachedTextureManager.GetCachedTexture(url, this.m_cachedTextureValidDays);
                    if (cachedTexture != null)
                    {
                        if (this.m_image != null)
                        {
                            this.m_image.SetSprite(Sprite.Create(cachedTexture, new Rect(0f, 0f, (float) cachedTexture.get_width(), (float) cachedTexture.get_height()), new Vector2(0.5f, 0.5f)), ImageAlphaTexLayout.None);
                            if (this.m_setNativeSize)
                            {
                                this.SetNativeSize();
                            }
                            this.m_httpImageState = enHttpImageState.Loaded;
                            if (this.m_loadingCover != null)
                            {
                                this.m_loadingCover.CustomSetActive(false);
                            }
                        }
                    }
                    else
                    {
                        base.StartCoroutine(this.DownloadImage(url));
                    }
                }
                else
                {
                    base.StartCoroutine(this.DownloadImage(url));
                }
            }
        }

        protected override void OnDestroy()
        {
            this.m_loadingCover = null;
            this.m_image = null;
            this.m_imageDefaultSprite = null;
            base.OnDestroy();
        }

        private void OnDisable()
        {
            if (base.m_isInitialized && (this.m_httpImageState == enHttpImageState.Loading))
            {
                base.StopAllCoroutines();
                this.m_httpImageState = enHttpImageState.Unload;
                if (this.m_loadingCover != null)
                {
                    this.m_loadingCover.CustomSetActive(true);
                }
            }
        }

        private void OnEnable()
        {
            if ((base.m_isInitialized && (this.m_httpImageState == enHttpImageState.Unload)) && !string.IsNullOrEmpty(this.m_imageUrl))
            {
                this.LoadTexture(this.m_imageUrl);
            }
        }

        public void SetImageSprite(string prefabPath, CUIFormScript formScript)
        {
            if (this.m_image != null)
            {
                this.m_image.SetSprite(prefabPath, formScript, true, false, false, false);
            }
        }

        public void SetImageUrl(string url)
        {
            if (this.m_forceSetImageUrl || !string.Equals(url, this.m_imageUrl))
            {
                this.m_imageUrl = url;
                if (this.m_image != null)
                {
                    this.m_image.SetSprite(this.m_imageDefaultSprite, this.m_imageDefaultAlphaTexLayout);
                }
                if (base.get_gameObject().get_activeInHierarchy() && (this.m_httpImageState == enHttpImageState.Loading))
                {
                    base.StopAllCoroutines();
                }
                this.m_httpImageState = enHttpImageState.Unload;
                if (this.m_loadingCover != null)
                {
                    this.m_loadingCover.CustomSetActive(true);
                }
                if (base.get_gameObject().get_activeInHierarchy() && !string.IsNullOrEmpty(this.m_imageUrl))
                {
                    this.LoadTexture(this.m_imageUrl);
                }
            }
        }

        public void SetNativeSize()
        {
            if (this.m_image != null)
            {
                this.m_image.SetNativeSize();
            }
        }

        [CompilerGenerated]
        private sealed class <DownloadImage>c__Iterator37 : IDisposable, IEnumerator, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string <$>url;
            internal CUIHttpImageScript <>f__this;
            internal string <contentType>__2;
            internal bool <isGif>__3;
            internal MemoryStream <memoryStream>__5;
            internal float <startTime>__0;
            internal Texture2D <texture2D>__4;
            internal WWW <www>__1;
            internal string url;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<>f__this.m_httpImageState = enHttpImageState.Loading;
                        this.<startTime>__0 = Time.get_realtimeSinceStartup();
                        this.<www>__1 = new WWW(this.url);
                        this.$current = this.<www>__1;
                        this.$PC = 1;
                        return true;

                    case 1:
                        this.<>f__this.m_httpImageState = enHttpImageState.Loaded;
                        if (!string.IsNullOrEmpty(this.<www>__1.get_error()))
                        {
                            goto Label_0274;
                        }
                        if (this.<>f__this.m_loadingCover != null)
                        {
                            this.<>f__this.m_loadingCover.CustomSetActive(false);
                        }
                        this.<contentType>__2 = null;
                        this.<www>__1.get_responseHeaders().TryGetValue("CONTENT-TYPE", out this.<contentType>__2);
                        if (this.<contentType>__2 != null)
                        {
                            this.<contentType>__2 = this.<contentType>__2.ToLower();
                        }
                        if (string.IsNullOrEmpty(this.<contentType>__2) || !this.<contentType>__2.Contains("image/"))
                        {
                            goto Label_0274;
                        }
                        this.<isGif>__3 = string.Equals(this.<contentType>__2, "image/gif");
                        this.<texture2D>__4 = null;
                        if (!this.<isGif>__3)
                        {
                            this.<texture2D>__4 = this.<www>__1.get_texture();
                            break;
                        }
                        this.<memoryStream>__5 = new MemoryStream(this.<www>__1.get_bytes());
                        try
                        {
                            this.<texture2D>__4 = GifHelper.GifToTexture(this.<memoryStream>__5, 0);
                        }
                        finally
                        {
                            if (this.<memoryStream>__5 != null)
                            {
                                this.<memoryStream>__5.Dispose();
                            }
                        }
                        break;

                    default:
                        goto Label_027B;
                }
                if (this.<texture2D>__4 != null)
                {
                    if (this.<>f__this.m_image != null)
                    {
                        this.<>f__this.m_image.SetSprite(Sprite.Create(this.<texture2D>__4, new Rect(0f, 0f, (float) this.<texture2D>__4.get_width(), (float) this.<texture2D>__4.get_height()), new Vector2(0.5f, 0.5f)), ImageAlphaTexLayout.None);
                        if (this.<>f__this.m_setNativeSize)
                        {
                            this.<>f__this.SetNativeSize();
                        }
                    }
                    if (this.<>f__this.m_cacheTexture)
                    {
                        CUIHttpImageScript.s_cachedTextureManager.AddCachedTexture(this.url, this.<texture2D>__4.get_width(), this.<texture2D>__4.get_height(), this.<isGif>__3, this.<www>__1.get_bytes());
                    }
                }
            Label_0274:
                this.$PC = -1;
            Label_027B:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

