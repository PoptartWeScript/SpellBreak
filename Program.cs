using System;
using System.Collections.Generic;
using SharpDX;
using WeScriptWrapper;
using WeScript.SDK.UI;
using WeScript.SDK.UI.Components;


namespace SpellBreak
{
    public class Program
    {
        public static IntPtr processHandle = IntPtr.Zero;
        public static IntPtr wndHnd = IntPtr.Zero;
        public static IntPtr GWorldPtr = IntPtr.Zero;
        public static IntPtr GNamesPtr = IntPtr.Zero;
        public static IntPtr FNamePool = IntPtr.Zero;
        public static IntPtr ULocalPlayerControler = IntPtr.Zero;
        public static IntPtr bKickbackEnabled = IntPtr.Zero;
        public static IntPtr LTeamNum = IntPtr.Zero;
        public static IntPtr TeamNum = IntPtr.Zero;
        public static IntPtr LAKSTeamState = IntPtr.Zero;
        public static IntPtr GameBase = IntPtr.Zero;
        public static IntPtr GameSize = IntPtr.Zero;
        public static IntPtr FindWindow = IntPtr.Zero;
        public static IntPtr ULevel = IntPtr.Zero;
        public static IntPtr AActors = IntPtr.Zero;
        public static IntPtr AActor = IntPtr.Zero;
        public static IntPtr USceneComponent = IntPtr.Zero;
        public static IntPtr actor_pawn = IntPtr.Zero;
        public static IntPtr Playerstate = IntPtr.Zero;
        public static IntPtr AKSTeamState = IntPtr.Zero;
        public static IntPtr UplayerState = IntPtr.Zero;
        public static IntPtr UGameInstance = IntPtr.Zero;
        public static IntPtr localPlayerArray = IntPtr.Zero;
        public static IntPtr ULocalPlayer = IntPtr.Zero;
        public static IntPtr Upawn = IntPtr.Zero;
        public static IntPtr APlayerCameraManager = IntPtr.Zero;
               
        public static bool gameProcessExists = false; //avoid drawing if the game process is dead, or not existent
        public static bool isWow64Process = true; //we all know the game is 32bit, but anyway...
        public static bool isGameOnTop = false; //we should avoid drawing while the game is not set on top
        public static bool isOverlayOnTop = false; //we might allow drawing visuals, while the user is working with the "menu"
        public static uint PROCESS_ALL_ACCESS = 0x1FFFFF; //hardcoded access right to OpenProcess (even EAC strips some of the access flags)
                
        public static float FMinimalViewInfo_FOV = 0;

        public static Dictionary<UInt32, string> CachedID = new Dictionary<UInt32, string>();

        public static uint ActorCnt = 0;
        public static uint AActorID = 0;
        public static uint calcPid = 0x1FFFFF;
        public static uint CharacterID = 0;
        public static uint DashID = 0;
        public static uint FeatherfallID = 0;
        public static uint InvisibilityID = 0;
        public static uint FlightID = 0;
        public static uint SpringstepID = 0;
        public static uint WolfsbloodID = 0;
        public static uint ShockID = 0;
        public static uint WindID = 0;
        public static uint EarthID = 0;
        public static uint FireID = 0;
        public static uint IceID = 0;
        public static uint PoisonID = 0;
        public static uint ShadowstepID = 0;
        public static uint TeleportationID = 0;
        public static uint ChronomasterID = 0;
        public static uint AmuletID = 0;
        public static uint BeltID = 0;
        public static uint BootsID = 0;
        public static uint LevelUpID = 0;
        public static uint ShrineID = 0;

        public static Vector2 wndMargins = new Vector2(0, 0); //if the game window is smaller than your desktop resolution, you should avoid drawing outside of it
        public static Vector2 wndSize = new Vector2(0, 0); //get the size of the game window ... to know where to draw
        public static Vector2 GameCenterPos = new Vector2(0, 0);
        public static Vector2 GameCenterPos2 = new Vector2(0, 0);
        public static Vector2 AimTarg2D = new Vector2(0, 0); //for aimbot

        public static Vector3 FMinimalViewInfo_Location = new Vector3(0, 0, 0);
        public static Vector3 FMinimalViewInfo_Rotation = new Vector3(0, 0, 0);
        public static Vector3 tempVec = new Vector3(0, 0, 0);
        public static Vector3 AimTarg3D = new Vector3(0, 0, 0);
        public static Menu RootMenu { get; private set; }
        public static Menu VisualsMenu { get; private set; }
        public static Menu MiscMenu { get; private set; }

        public static Menu AimbotMenu { get; private set; }


        class Components
        {
            public static readonly MenuKeyBind MainAssemblyToggle = new MenuKeyBind("mainassemblytoggle", "Toggle the whole assembly effect by pressing key:", VirtualKeyCode.Delete, KeybindType.Toggle, true);
            public static class VisualsComponent
            {
                public static readonly MenuBool DrawTheVisuals = new MenuBool("drawthevisuals", "Enable all of the Visuals", true);
                public static readonly MenuBool DrawTheItems = new MenuBool("drawtheitems", "Enable all of the items", true);
                public static readonly MenuColor CharacterColor = new MenuColor("srvcolor", "Survivors Color", new SharpDX.Color(0, 255, 0, 60));
                public static readonly MenuBool DrawCharacterBox = new MenuBool("srvbox", "Draw Survivors Box", true);
                public static readonly MenuSlider DrawBoxThic = new MenuSlider("boxthickness", "Draw Box Thickness", 0, 0, 10);
                public static readonly MenuBool DrawBoxBorder = new MenuBool("drawboxborder", "Draw Border around Box and Text?", true);
            }
            public static class AimbotComponentSurvivor
            {

                public static readonly MenuBool AimGlobalBool = new MenuBool("enableaim", "Enable Aimbot Features", true);
                public static readonly MenuKeyBind AimKey = new MenuKeyBind("aimkey", "Aimbot HotKey (HOLD)", VirtualKeyCode.LeftMouse, KeybindType.Hold, false);
                public static readonly MenuSlider AimSpeed = new MenuSlider("aimspeed", "Aimbot Speed %", 12, 1, 100);
                public static readonly MenuSlider Distance = new MenuSlider("Distance Killer", "Distance kill%", 12, 1, 100);
                public static readonly MenuSlider AimFov = new MenuSlider("aimfov", "Aimbot FOV", 100, 4, 1000);
                public static readonly MenuBool DrawFov = new MenuBool("DrawFOV", "Enable FOV Circle Features Survivor", true);


            }
            public static class AimbotComponent
            {
                public static readonly MenuBool AimGlobalBool = new MenuBool("enableaim", "Enable Aimbot Features", true);
                public static readonly MenuKeyBind AimKey = new MenuKeyBind("aimkey", "Aimbot HotKey (HOLD)", VirtualKeyCode.LeftMouse, KeybindType.Hold, false);
                public static readonly MenuSlider AimSpeed = new MenuSlider("aimspeed", "Aimbot Speed %", 12, 1, 100);
                public static readonly MenuSlider Distance = new MenuSlider("Distance", "Distance kill%", 12, 1, 100);
                public static readonly MenuSlider AimFov = new MenuSlider("aimfov", "Aimbot FOV", 100, 4, 1000);
                public static readonly MenuBool DrawFov = new MenuBool("DrawFOV", "Enable FOV Circle Features", true);
                public static readonly MenuColor AimFovColor = new MenuColor("aimfovcolor", "FOV Color", new SharpDX.Color(255, 255, 255, 30));              
            }
        }

        public static void InitializeMenu()
        {
            VisualsMenu = new Menu("visualsmenu", "Visuals Menu")
            {
                Components.VisualsComponent.DrawTheVisuals,
                Components.VisualsComponent.DrawTheItems,
                Components.VisualsComponent.CharacterColor,
                Components.VisualsComponent.DrawCharacterBox,
                Components.VisualsComponent.DrawBoxThic.SetToolTip("Setting thickness to 0 will let the assembly auto-adjust itself depending on model distance"),
                Components.VisualsComponent.DrawBoxBorder.SetToolTip("Drawing borders may take extra performance (FPS) on low-end computers"),
            };

            AimbotMenu = new Menu("aimbotmenu", "Aimbot Menu")
            {
                Components.AimbotComponent.AimGlobalBool,
                Components.AimbotComponent.AimKey,
                Components.AimbotComponent.AimSpeed,
                Components.AimbotComponent.AimFov,
                Components.AimbotComponent.DrawFov,
                Components.AimbotComponent.AimFovColor,
            };

            RootMenu = new Menu("SpellBreak", "WeScript.app SpellBreak Assembly", true)
            {
                Components.MainAssemblyToggle.SetToolTip("The magical boolean which completely disables/enables the assembly!"),
                VisualsMenu,
                MiscMenu,
                AimbotMenu,

            };
            RootMenu.Attach();
        }
        private static double GetDistance2D(Vector2 pos1, Vector2 pos2)
        {
            Vector2 vector = new Vector2(pos1.X - pos2.X, pos1.Y - pos2.Y);
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }
        public static void SigScan()
        {
            GWorldPtr = Memory.ZwReadPointer(processHandle, GameBase + 0x738BEB8, isWow64Process);
            GNamesPtr = GameBase + 0x7404700;
        }
        private static string GetNameFromID(uint ID)
        {
            var GNamesAddress = Memory.ZwReadPointer(processHandle, (IntPtr)(GNamesPtr.ToInt64() + 0x0), isWow64Process);
            if (GNamesAddress != IntPtr.Zero)
            {
                UInt64 ChunkIndex = ID / 0x4000;
                UInt64 WithinChunkIndex = ID % 0x4000;
                var fNamePtr = Memory.ZwReadPointer(processHandle, (IntPtr)(GNamesAddress.ToInt64() + (long)ChunkIndex * 0x8), isWow64Process);
                if (fNamePtr != IntPtr.Zero)
                {
                    var fName = Memory.ZwReadPointer(processHandle, (IntPtr)(fNamePtr.ToInt64() + 0x8 * (long)WithinChunkIndex), isWow64Process);
                    if (fName != IntPtr.Zero)
                    {
                        var name = Memory.ZwReadString(processHandle, (IntPtr)fName.ToInt64() + 0xC, false, 64);
                        if (name.Length > 0) return name;
                    }
                }
            }
            return "NULL";
        }

        static void Main(string[] args)
        {
            Console.WriteLine("WeScript.app experimental SpellBreak Assembly By Poptart");      
            InitializeMenu();
            if (!Memory.InitDriver(DriverName.nsiproxy))
            {
                Console.WriteLine("[ERROR] Failed to initialize driver for some reason...");
            }
            Renderer.OnRenderer += OnRenderer;
            Memory.OnTick += OnTick;
        }

        public static double dims = 0.01905f;
        private static double GetDistance3D(Vector3 myPos, Vector3 enemyPos)
        {
            Vector3 vector = new Vector3(myPos.X - enemyPos.X, myPos.Y - enemyPos.Y, myPos.Z - enemyPos.Z);
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z) * dims;
        }
        private static void OnTick(int counter, EventArgs args)
        {
            if (FindWindow == IntPtr.Zero) FindWindow = Memory.FindWindowName("Spellbreak  ");
            if (FindWindow != IntPtr.Zero)
            {
                calcPid = Memory.GetPIDFromHWND(FindWindow);
                gameProcessExists = true;
                if (processHandle == IntPtr.Zero) processHandle = Memory.ZwOpenProcess(PROCESS_ALL_ACCESS, calcPid);
                if (processHandle != IntPtr.Zero)
                {
                    if (GameBase == IntPtr.Zero) GameBase = Memory.ZwGetModule(processHandle, null, isWow64Process);
                    if (GameBase != IntPtr.Zero)
                    {
                        GameSize = Memory.ZwGetModuleSize(processHandle, null, isWow64Process);
                        if (GWorldPtr == IntPtr.Zero && GNamesPtr == IntPtr.Zero) SigScan();
                        if (GWorldPtr != IntPtr.Zero && GNamesPtr != IntPtr.Zero)
                        {
                            wndMargins = Renderer.GetWindowMargins(FindWindow);
                            wndSize = Renderer.GetWindowSize(FindWindow);
                            isGameOnTop = Renderer.IsGameOnTop(FindWindow);
                            GameCenterPos = new Vector2(wndSize.X / 2 + wndMargins.X, wndSize.Y / 2 + wndMargins.Y);
                            GameCenterPos2 = new Vector2(wndSize.X / 2 + wndMargins.X, wndSize.Y / 2 + wndMargins.Y + 750.0f);//even if the game is windowed, calculate perfectly it's "center" for aim or crosshair
                            isOverlayOnTop = Overlay.IsOnTop();
                        }
                    }
                }

            }
        }
        private static void OnRenderer(int fps, EventArgs args)
        {
            if (!gameProcessExists) return; //process is dead, don't bother drawing
            if ((!isGameOnTop) && (!isOverlayOnTop)) return; //if game and overlay are not on top, don't draw
            if (!Components.MainAssemblyToggle.Enabled) return; //main menu boolean to toggle the cheat on or off
            GameCenterPos = new Vector2(wndSize.X / 2 + wndMargins.X, wndSize.Y / 2 + wndMargins.Y);
            double fClosestPos = 999999;
            if (GWorldPtr != IntPtr.Zero)
            {
                Functions.Ppc();
                ULevel = Memory.ZwReadPointer(processHandle, GWorldPtr + 0x38, isWow64Process);
                if (ULevel != IntPtr.Zero)
                {
                    AActors = Memory.ZwReadPointer(processHandle, (IntPtr)ULevel.ToInt64() + 0xA0, isWow64Process);
                    ActorCnt = Memory.ZwReadUInt32(processHandle, (IntPtr)ULevel.ToInt64() + 0xA8);
                    if ((AActors != IntPtr.Zero) && (ActorCnt > 0))
                    {
                        for (uint i = 0; i <= ActorCnt; i++)
                        {
                            AActor = Memory.ZwReadPointer(processHandle, (IntPtr)(AActors.ToInt64() + i * 8),
                                isWow64Process);
                            if (AActor != IntPtr.Zero)
                            {
                                AActorID = Memory.ZwReadUInt32(processHandle, (IntPtr)AActor.ToInt64() + 0x18);
                                if (!CachedID.ContainsKey(AActorID))
                                {
                                    var retname = GetNameFromID(AActorID);
                                    CachedID.Add(AActorID, retname);
                                }
                                USceneComponent = Memory.ZwReadPointer(processHandle, (IntPtr)AActor.ToInt64() + 0x168, isWow64Process);
                                if (USceneComponent != IntPtr.Zero)
                                {
                                    if ((AActorID > 0)) //&& (AActorID < 700000)
                                    {
                                        var retname = CachedID[AActorID];
                                        retname = GetNameFromID(AActorID);
                                        if (retname.Contains("PlayerCharacter")) CharacterID = AActorID;
                                        //if (retname.Contains("TrainingDummy")) CharacterID = AActorID;
                                        if (retname.Contains("Dash_Tier_5_C")) DashID = AActorID;
                                        if (retname.Contains("Featherfall_Tier_5_C")) FeatherfallID = AActorID;
                                        if (retname.Contains("Invisibility_Tier_5_C")) InvisibilityID = AActorID;
                                        if (retname.Contains("Flight_Tier_5_C")) FlightID = AActorID;
                                        if (retname.Contains("Springstep_Tier_5_C")) SpringstepID = AActorID;
                                        if (retname.Contains("Wolfsblood_Tier_5_C")) WolfsbloodID = AActorID;
                                        if (retname.Contains("Shock_Tier_5_C")) ShockID = AActorID;
                                        if (retname.Contains("Wind_Tier_5_C")) WindID = AActorID;
                                        if (retname.Contains("Earth_Tier_5_C")) EarthID = AActorID;
                                        if (retname.Contains("Fire_Tier_5_C")) FireID = AActorID;
                                        if (retname.Contains("Ice_Tier_5_C")) IceID = AActorID;
                                        if (retname.Contains("Poison_Tier_5_C")) PoisonID = AActorID;
                                        if (retname.Contains("Shadowstep_Tier_5_C")) ShadowstepID = AActorID;
                                        if (retname.Contains("Teleportation_Tier_5_C")) TeleportationID = AActorID;
                                        if (retname.Contains("Chronomaster_Tier_5_C")) ChronomasterID = AActorID;
                                        if (retname.Contains("Amulet_Tier_5_C")) AmuletID = AActorID;
                                        if (retname.Contains("Belt_Tier_5_C")) BeltID = AActorID;
                                        if (retname.Contains("Boots_Tier_5_C")) BootsID = AActorID;
                                        if (retname.Contains("LevelUp_C")) LevelUpID = AActorID;
                                        if (retname.Contains("ShrineMapMark_C")) ShrineID = AActorID;

                                    }
                                    tempVec = Memory.ZwReadVector3(processHandle, (IntPtr)USceneComponent.ToInt64() + 0x17C);

                                    int dist = (int)(GetDistance3D(FMinimalViewInfo_Location, tempVec));

                                    if (Components.VisualsComponent.DrawTheVisuals.Enabled)
                                    {
                                        if (AActorID == CharacterID)
                                        {
                                            Vector2 vScreen_h3adSurvivor = new Vector2(0, 0);
                                            Vector2 vScreen_f33tSurvivor = new Vector2(0, 0);
                                            if (Renderer.WorldToScreenUE4(new Vector3(tempVec.X, tempVec.Y, tempVec.Z + 85), out vScreen_h3adSurvivor, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                            {

                                                Renderer.WorldToScreenUE4(new Vector3(tempVec.X, tempVec.Y, tempVec.Z - 100.0f), out vScreen_f33tSurvivor, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize);

                                                if (Components.AimbotComponent.DrawFov.Enabled)
                                                {
                                                    Renderer.DrawCircle(GameCenterPos, Components.AimbotComponent.AimFov.Value, Color.White);
                                                }

                                                if (Components.VisualsComponent.DrawCharacterBox.Enabled && dist > 10)
                                                {
                                                    Renderer.DrawLine(GameCenterPos2, vScreen_h3adSurvivor, Components.VisualsComponent.CharacterColor.Color, Components.VisualsComponent.DrawBoxThic.Value);
                                                    Renderer.DrawFPSBox(vScreen_h3adSurvivor, vScreen_f33tSurvivor, Components.VisualsComponent.CharacterColor.Color, BoxStance.standing, Components.VisualsComponent.DrawBoxThic.Value, Components.VisualsComponent.DrawBoxBorder.Enabled);
                                                    Renderer.DrawText("[" + dist + "m]", vScreen_h3adSurvivor.X, vScreen_f33tSurvivor.Y, Color.White, 12, TextAlignment.centered, false);
                                                }

                                                var AimDist2D = GetDistance2D(vScreen_h3adSurvivor, GameCenterPos);
                                                if (Components.AimbotComponent.AimFov.Value < AimDist2D) continue;

                                                if (AimDist2D < fClosestPos)
                                                {
                                                    fClosestPos = AimDist2D;
                                                    AimTarg2D = vScreen_h3adSurvivor;
                                                    if (Components.AimbotComponent.AimKey.Enabled && Components.AimbotComponent.AimGlobalBool.Enabled && dist > 10)
                                                    {

                                                        double DistX = 0;
                                                        double DistY = 0;
                                                        DistX = (AimTarg2D.X) - GameCenterPos.X;
                                                        DistY = (AimTarg2D.Y) - GameCenterPos.Y;

                                                        double slowDistX = DistX / (1.0f + (Math.Abs(DistX) / (1.0f + Components.AimbotComponent.AimSpeed.Value)));
                                                        double slowDistY = DistY / (1.0f + (Math.Abs(DistY) / (1.0f + Components.AimbotComponent.AimSpeed.Value)));
                                                        Input.mouse_eventWS(MouseEventFlags.MOVE, (int)slowDistX, (int)slowDistY, MouseEventDataXButtons.NONE, IntPtr.Zero);
                                                    }

                                                }
                                            }
                                        }

                                        if (Components.VisualsComponent.DrawTheItems.Enabled && dist <= 300)
                                        {
                                            if (AActorID == DashID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(new Vector3(tempVec.X, tempVec.Y, tempVec.Z + 50.0f), out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {

                                                    Renderer.DrawText("Dash[" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == FeatherfallID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(new Vector3(tempVec.X, tempVec.Y, tempVec.Z + 50.0f), out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {

                                                    Renderer.DrawText("Featherfall[" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == InvisibilityID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(new Vector3(tempVec.X, tempVec.Y, tempVec.Z + 50.0f), out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText("Invisibility[" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == FlightID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(new Vector3(tempVec.X, tempVec.Y, tempVec.Z + 50.0f), out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText("Flight[" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == SpringstepID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Springstep [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == WolfsbloodID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Wolfsblood [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == ShockID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Shock [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == WindID)
                                            {
                                                Vector2 vScreen_d3d11bb = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11bb, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Wind [" + dist + "m]", vScreen_d3d11bb, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == EarthID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Earth [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == FireID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Fire [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == IceID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Ice [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == PoisonID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Poison [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == ShadowstepID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Shadow [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == TeleportationID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Teleportation [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == ChronomasterID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Chrono [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == AmuletID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Amulet [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == BeltID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Belt [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == BootsID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Boots [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == LevelUpID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" LevelUP [" + dist + "m]", vScreen_d3d11, Color.Orange, 12);
                                                }
                                            }

                                            if (AActorID == ShrineID)
                                            {
                                                Vector2 vScreen_d3d11 = new Vector2(0, 0);
                                                if (Renderer.WorldToScreenUE4(tempVec, out vScreen_d3d11, FMinimalViewInfo_Location, FMinimalViewInfo_Rotation, FMinimalViewInfo_FOV, wndMargins, wndSize))
                                                {
                                                    Renderer.DrawText(" Shrine [" + dist + "m]", vScreen_d3d11, Color.HotPink, 12);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
