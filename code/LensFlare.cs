﻿using Sandbox;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Sandbox
{
    [SceneCamera.AutomaticRenderHook]
    public partial class LensFlare : RenderHook
    {
        /*
         * Lens Flare Post-Processing Effect
         * By Mahalis (revised by Rush_Freak)
         * original from GMod, adapted for S&Box by Gmod4phun
         */

        RenderAttributes attributes = new RenderAttributes();
        Material mat = Material.FromShader("ui_additive");
        Texture texIris = Texture.Load(FileSystem.Mounted, "materials/lensflare/iris.png");
        Texture texFlare = Texture.Load(FileSystem.Mounted, "materials/lensflare/flare.png");
        Texture texRing = Texture.Load(FileSystem.Mounted, "materials/lensflare/color_ring.png");
        Texture texBar = Texture.Load(FileSystem.Mounted, "materials/lensflare/bar.png");

        private float mulW(float x, float f)
        {
            return (x - Screen.Width / 2) * f + Screen.Width / 2;
        }

        private float mulH(float y, float f)
        {
            return (y - Screen.Height / 2) * f + Screen.Height / 2;
        }

        private void CenteredSprite(float x, float y, float sz, Color col)
        {
            var r = new Rect(x - sz / 2, y - sz / 2, sz, sz);
            Graphics.DrawQuad(r, mat, col, attributes);
        }

        public override void OnStage(SceneCamera target, Stage renderStage)
        {
            if (renderStage != Stage.AfterPostProcess) // only render after PostProcess
                return;

            var sun = Entity.All.OfType<EnvironmentLightEntity>().FirstOrDefault();
            if (!sun.IsValid())
                return;

            var eyepos = target.Position;
            var eyevector = target.Rotation.Forward;
            var sundirection = sun.Rotation.Backward.Normal;
            var sunobstruction = 1f;

            var tr = Trace.Ray(eyepos, eyepos + sundirection * 128000).WithTag("solid").Run();
            if (tr.Hit)
                sunobstruction = 0f;

            var sunPos3D = eyepos + sundirection * target.ZFar;
            var sunPos2D = sunPos3D.ToScreen(Screen.Size);
            if (sunPos2D == null)
                return;

            var sunpos = (Vector2)sunPos2D;

            var rSz = Screen.Width * 0.1f;
            var aMul = (float)((sundirection.Dot(eyevector) - 0.4f) * (1 - Math.Pow(1 - sunobstruction, 2))).Clamp(0f, 1f);

            if (aMul == 0f)
                return;

            var aMulPow3 = (float)Math.Pow(aMul, 3);
            var colorIris = Color.FromBytes(255, 230, 180, (int)(255 * aMulPow3));

            attributes.Set("Texture", texRing);
            CenteredSprite(mulW(sunpos.x, 0.5f), mulH(sunpos.y, 0.5f), rSz * 8, Color.White.WithAlpha(800 * aMulPow3));

            attributes.Set("Texture", texFlare);
            CenteredSprite(sunpos.x, sunpos.y, rSz * 10, Color.White.WithAlpha(800 * aMulPow3));

            attributes.Set("Texture", texBar);
            CenteredSprite(mulW(sunpos.x, 1), mulH(sunpos.y, 1), rSz * 5, Color.White.WithAlpha(255f * aMul));

            attributes.Set("Texture", texIris);
            CenteredSprite(mulW(sunpos.x, 1.8f), mulH(sunpos.y, 1.8f), rSz * 0.15f, colorIris);
            CenteredSprite(mulW(sunpos.x, 1.82f), mulH(sunpos.y, 1.82f), rSz * 0.1f, colorIris);
            CenteredSprite(mulW(sunpos.x, 1.5f), mulH(sunpos.y, 1.5f), rSz * 0.05f, colorIris);
            CenteredSprite(mulW(sunpos.x, 0.6f), mulH(sunpos.y, 0.6f), rSz * 0.05f, colorIris);
            CenteredSprite(mulW(sunpos.x, 0.59f), mulH(sunpos.y, 0.59f), rSz * 0.15f, colorIris);
            CenteredSprite(mulW(sunpos.x, 0.3f), mulH(sunpos.y, 0.3f), rSz * 0.1f, colorIris);
            CenteredSprite(mulW(sunpos.x, -0.7f), mulH(sunpos.y, -0.7f), rSz * 0.1f, colorIris);
            CenteredSprite(mulW(sunpos.x, -0.72f), mulH(sunpos.y, -0.72f), rSz * 0.15f, colorIris);
            CenteredSprite(mulW(sunpos.x, -0.73f), mulH(sunpos.y, -0.73f), rSz * 0.05f, colorIris);
            CenteredSprite(mulW(sunpos.x, -0.9f), mulH(sunpos.y, -0.9f), rSz * 0.1f, colorIris);
            CenteredSprite(mulW(sunpos.x, -0.92f), mulH(sunpos.y, -0.92f), rSz * 0.05f, colorIris);
            CenteredSprite(mulW(sunpos.x, -1.3f), mulH(sunpos.y, -1.3f), rSz * 0.15f, colorIris);
            CenteredSprite(mulW(sunpos.x, -1.5f), mulH(sunpos.y, -1.5f), rSz * 1f, colorIris);
            CenteredSprite(mulW(sunpos.x, -1.7f), mulH(sunpos.y, -1.7f), rSz * 0.1f, colorIris);
        }
    }
}
