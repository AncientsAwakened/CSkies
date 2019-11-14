using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace CSkies.Items.Observatory
{
    public class EnigmaLog1 : ModItem
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enigma Log #1");
            Tooltip.SetDefault(
@"Entry log; I have arrived on planet T-3R in the milky way galaxy.
Or as the lesser beings call it, '" + Main.worldName + @"'. What a stupid name.
If my reading are correct...the abyss vaults should be here somewhere. I'll
keep looking. Shouldn't take too long...");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 32;
            item.maxStack = 1;
            item.rare = 7;
            item.value = 0;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Color.LimeGreen.ToVector3() * 0.55f * Main.essScale);
        }

        static int counter = 0;
        static int cframe = 0;

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (counter++ > 4)
            {
                cframe++;
                counter = 0;
                if (cframe > 3)
                {
                    cframe = 0;
                }
            }
            Texture2D Tex = Main.itemTexture[item.type];

            Texture2D Glow = mod.GetTexture("Glowmasks/EnigmaLog_Glow");

            Rectangle iframe = BaseDrawing.GetFrame(cframe, Tex.Width, Tex.Height / 4, 0, 0);

            BaseDrawing.DrawTexture(spriteBatch, Tex, 0, item.position, item.width, item.height, scale, rotation, item.direction, 11, iframe, lightColor, true);
            BaseDrawing.DrawTexture(spriteBatch, Glow, 0, item.position, item.width, item.height, scale, rotation, item.direction, 11, iframe, Colors.COLOR_GLOWPULSE, true);
            return false;
        }
    }

    public class EnigmaLog2 : EnigmaLog1
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Enigma Log #2");
            Tooltip.SetDefault(
@"What a waste of my time this hunk of cybernetic cess
was, I never even turned it on. Didn't deserve
my time. Why haven't the S.W.E.E.P.E.R. Units
dumped it into the ocean yet?! Good help these
days is so hard to find...");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class EnigmaLog3 : EnigmaLog1
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Enigma Log #3");
            Tooltip.SetDefault(
@"I have discovered one of the abyss vaults. Just floating
in space. Inanis, old chap, you didn't even try to hide it from
me! How dim-witted. Though I would expect no less from a lesser
being.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class EnigmaLog4 : EnigmaLog1
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Enigma Log #4");
            Tooltip.SetDefault(
@"Damn you, Inanis! I should have realized...
Locks. On Vaults. How did I not forsee something this basic..?
And these are no regular locks...I've never seen magic like 
this before...except for...
 
Inanis I swear to Galaxius if you--");

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class EnigmaLog5 : EnigmaLog1
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Enigma Log #5");
            Tooltip.SetDefault(
@"Inanis you bastard!!! You've made my job getting these
oversized puzzle cubes open about fourty times more taxing
on my supreme mind! Looks like I'll have to get an Abyss
Gate open somehow...how do you do that again..?");

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class EnigmaLog6 : EnigmaLog1
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Enigma Log #6");
            Tooltip.SetDefault(
@"'An abyss gate can only be created through the spacial
flux caused by the death of a supreme titan'
Supreme titan, eh..? Well lucky me, I've heard rumors of
just that somewhere here on T-3R. Some kind of ugly squid
giant...what was it called? The Moon Lord if I remember
correctly...now where to find this oversized calamari dish...");

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class EnigmaLog7 : EnigmaLog1
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Enigma Log #7");
            Tooltip.SetDefault(
@"So according to a fellow cybernetically enhanced individual
I stumbled across earlier today, the Moon Lord hasn't been
seen in millenia...worshipped only by some raving cultists
that hovel up at that oversized crypt on the surface...but
they've been chased off by that old coot at the entrance
whining about being 'Cursed'. Oh boo hoo you're cursed, I'm
shedding a very real tear down my titanium-alloy cheek.");

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class EnigmaLog8 : EnigmaLog1
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Enigma Log #8");
            Tooltip.SetDefault(
@"Since I'm going to be here for a very long time getting this
giant squid clown to show up, I might as well deploy Starcore.
Getting an eye in the sky from orbit should help me keep an eye
out for potential threats. Off you go, you magnificent machine.");

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class EnigmaLog9 : EnigmaLog1
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Enigma Log #9");
            Tooltip.SetDefault(
@"HOLY SWEET MOTHER OF GEMINI!!!
A comet just whizzed past my workshop and slammed into the surface.
It was probably one of those pestilent Observers. WATCH WHERE YOU
FLY YOU OVERSIZED SAMPLE OF RETINAL CANCER!!! Why didn't Starcore
warn me--

...oh.

...7 warnings from SC-01.

...shite.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class EnigmaLog10 : EnigmaLog1
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Enigma Log #10");
            Tooltip.SetDefault(
@"Hm? Starcore is picking up something slaughtering strong creatures
left and right..? That can't be right...the natives can barely fend 
off a simple slime...Whatever. It shouldn't be too much of a--
...What? Is that...my calling signal? How did they...whatever. Too 
much of a risk. Starcore, exterminate the source of that signal with 
extreme prejudice. Someone replicating my call signal is...concerning.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class EnigmaLog11 : EnigmaLog1
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Enigma Log #11");
            Tooltip.SetDefault(
@"'Starcore Unit Fatally Damaged. Recovery Impossible'

WHAT?! HOW DID THEY-- MY BEAUTIFUL STARCORE!

...who or whatever you are, this just got personal. If Starcore couldn't deal 
with them....appears it's time to take matters into my own hands. Signing off. I
have a vandal to vaporize.

-Logout Complete.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class EnigmaLogAA : EnigmaLog1
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Enigma Log Extra I");
            Tooltip.SetDefault(
@"Those dark islands to the east...the contraptions there quite facinate me. 
However, I refuse to go back over there. Not after I got trampled by that 
equestrian annoyance. My back prosthetic is still dented from getting stepped on!");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    public class EnigmaLogRede : EnigmaLog1
    {
        public override string Texture => "CSkies/Items/Logs/EnigmaLog";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Enigma Log Extra II");
            Tooltip.SetDefault(@"Those damn T-Bots! They are absolutely stealing
my style! My color scheme, my robotic designs, everything!
Plagiarism I say, Plagiarism!!!
 
Huh?

You mean they've been here for how long?
 
...well shit.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }
}