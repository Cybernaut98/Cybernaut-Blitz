namespace Blitzcrank
{
    using System;
    using System.Linq;

    using EnsoulSharp;
    using EnsoulSharp.SDK;
    using EnsoulSharp.SDK.MenuUI;
    using EnsoulSharp.SDK.MenuUI.Values;
    using EnsoulSharp.SDK.Prediction;
    using EnsoulSharp.SDK.Utility;

    using Color = System.Drawing.Color;

    public class Program
    {
        private static Menu MainMenu;

        private static Spell Q;
        private static Spell W;
        private static Spell E;
        private static Spell R;

        private static void Main(string[] args)
        {
            GameEvent.OnGameLoad += OnGameLoad;
        }

        private static void OnGameLoad()
        {
            if (ObjectManager.Player.CharacterName != "Blitzcrank")
            {
                return;
            }

            Q = new Spell(SpellSlot.Q, 925f);
            Q.SetSkillshot(0.25f, 80f, float.MaxValue, false, SkillshotType.Line);

            W = new Spell(SpellSlot.W, 1000f);

            E = new Spell(SpellSlot.E, 1000f);

            R = new Spell(SpellSlot.R, 550f);


            MainMenu = new Menu("cyberblitz", "Cyber Blitz", true);

            var comboMenu = new Menu("Combo", "Combo Settings");
            comboMenu.Add(new MenuBool("comboQ", "Use Q"));
            comboMenu.Add(new MenuBool("comboW", "Use W"));
            comboMenu.Add(new MenuBool("comboE", "Use E"));
            comboMenu.Add(new MenuBool("comboR", "Use R"));
            MainMenu.Add(comboMenu);

            var harassMenu = new Menu("Harass", "Harass Settings");
            harassMenu.Add(new MenuBool("harassQ", "Use Q"));
            harassMenu.Add(new MenuBool("harassW", "Use W"));
            harassMenu.Add(new MenuBool("harassE", "Use E"));
            MainMenu.Add(harassMenu);

            var drawMenu = new Menu("Draw", "Draw Settings");
            drawMenu.Add(new MenuBool("drawQ", "Draw Q Range"));
            drawMenu.Add(new MenuBool("drawR", "Draw R Range"));
            MainMenu.Add(drawMenu);


            MainMenu.Add(new MenuBool("isDead", "if Player is Dead not Draw Range"));
            MainMenu.Attach();

            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
        }

        private static void Combo()
        {
            if (MainMenu["Combo"]["comboQ"].GetValue<MenuBool>().Enabled && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

                if (target != null && target.IsValidTarget(Q.Range))
                {
                    var pred = Q.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        Q.Cast(pred.CastPosition);
                    }
                }
            }

            if (MainMenu["Combo"]["comboW"].GetValue<MenuBool>().Enabled && W.IsReady())
            {
                W.Cast();
            }

            if (MainMenu["Combo"]["comboE"].GetValue<MenuBool>().Enabled && E.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, DamageType.Physical);

                if (target != null && target.IsValidTarget(E.Range))
                {
                    ;
                }

                {
                    E.CastOnUnit(target);
                }
            }

            if (MainMenu["Combo"]["comboR"].GetValue<MenuBool>().Enabled && R.IsReady())
            {
                var target = TargetSelector.GetTarget(R.Range, DamageType.Physical);

                if (target != null && target.IsValidTarget(R.Range))
                {
                    
                }
                {
                    R.CastOnUnit(target);
                }
            }
        }
    
                
            
        

        private static void Clear()
        {
            // check out Ashe or Kalista
            // it already have example

            // get Minion
            var minions = GameObjects.EnemyMinions.Where(x => x.IsValidTarget(Q.Range) && x.IsMinion());

            // get Mob
            var mobs = GameObjects.Jungle.Where(x => x.IsValidTarget(Q.Range));

            // get Legendary Mob (Dragon, Baron, ect)
            var lMobs = GameObjects.Jungle.Where(x => x.IsValidTarget(Q.Range) && x.GetJungleType() == JungleType.Legendary);

            // get Large Mob (Red Buff, Blue Buff, ect)
            var bMobs = GameObjects.Jungle.Where(x => x.IsValidTarget(Q.Range) && x.GetJungleType() == JungleType.Large);
        }

        private static void OnUpdate(EventArgs args)
        {
            switch (Orbwalker.ActiveMode)
            {
                case OrbwalkerMode.Combo:
                    Combo();
                    break;
                case OrbwalkerMode.LaneClear:
                    Clear();
                    break;
            }
        }

        private static void OnDraw(EventArgs args)
        {
            // if Player is Dead not Draw Range
            if (MainMenu["isDead"].GetValue<MenuBool>().Enabled)
            {
                if (ObjectManager.Player.IsDead)
                {
                    return;
                }
            }

            // draw Q Range
            if (MainMenu["Draw"]["drawQ"].GetValue<MenuBool>().Enabled)
            {
                // Draw Circle
                Render.Circle.DrawCircle(ObjectManager.Player.Position, Q.Range, Color.Aqua);
                Render.Circle.DrawCircle(ObjectManager.Player.Position, R.Range, Color.White);
            }
        }
    }
}