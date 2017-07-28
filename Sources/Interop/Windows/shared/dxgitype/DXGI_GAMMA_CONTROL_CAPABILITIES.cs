// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct DXGI_GAMMA_CONTROL_CAPABILITIES
    {
        #region Fields
        [ComAliasName("BOOL")]
        public int ScaleAndOffsetSupported;

        public float MaxConvertedValue;

        public float MinConvertedValue;

        [ComAliasName("UINT")]
        public uint NumGammaControlPoints;

        public _ControlPointPositions_e__FixedBuffer ControlPointPositions;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _ControlPointPositions_e__FixedBuffer
        {
            #region Fields
            public float e0;

            public float e1;

            public float e2;

            public float e3;

            public float e4;

            public float e5;

            public float e6;

            public float e7;

            public float e8;

            public float e9;

            public float e10;

            public float e11;

            public float e12;

            public float e13;

            public float e14;

            public float e15;

            public float e16;

            public float e17;

            public float e18;

            public float e19;

            public float e20;

            public float e21;

            public float e22;

            public float e23;

            public float e24;

            public float e25;

            public float e26;

            public float e27;

            public float e28;

            public float e29;

            public float e30;

            public float e31;

            public float e32;

            public float e33;

            public float e34;

            public float e35;

            public float e36;

            public float e37;

            public float e38;

            public float e39;

            public float e40;

            public float e41;

            public float e42;

            public float e43;

            public float e44;

            public float e45;

            public float e46;

            public float e47;

            public float e48;

            public float e49;

            public float e50;

            public float e51;

            public float e52;

            public float e53;

            public float e54;

            public float e55;

            public float e56;

            public float e57;

            public float e58;

            public float e59;

            public float e60;

            public float e61;

            public float e62;

            public float e63;

            public float e64;

            public float e65;

            public float e66;

            public float e67;

            public float e68;

            public float e69;

            public float e70;

            public float e71;

            public float e72;

            public float e73;

            public float e74;

            public float e75;

            public float e76;

            public float e77;

            public float e78;

            public float e79;

            public float e80;

            public float e81;

            public float e82;

            public float e83;

            public float e84;

            public float e85;

            public float e86;

            public float e87;

            public float e88;

            public float e89;

            public float e90;

            public float e91;

            public float e92;

            public float e93;

            public float e94;

            public float e95;

            public float e96;

            public float e97;

            public float e98;

            public float e99;

            public float e100;

            public float e101;

            public float e102;

            public float e103;

            public float e104;

            public float e105;

            public float e106;

            public float e107;

            public float e108;

            public float e109;

            public float e110;

            public float e111;

            public float e112;

            public float e113;

            public float e114;

            public float e115;

            public float e116;

            public float e117;

            public float e118;

            public float e119;

            public float e120;

            public float e121;

            public float e122;

            public float e123;

            public float e124;

            public float e125;

            public float e126;

            public float e127;

            public float e128;

            public float e129;

            public float e130;

            public float e131;

            public float e132;

            public float e133;

            public float e134;

            public float e135;

            public float e136;

            public float e137;

            public float e138;

            public float e139;

            public float e140;

            public float e141;

            public float e142;

            public float e143;

            public float e144;

            public float e145;

            public float e146;

            public float e147;

            public float e148;

            public float e149;

            public float e150;

            public float e151;

            public float e152;

            public float e153;

            public float e154;

            public float e155;

            public float e156;

            public float e157;

            public float e158;

            public float e159;

            public float e160;

            public float e161;

            public float e162;

            public float e163;

            public float e164;

            public float e165;

            public float e166;

            public float e167;

            public float e168;

            public float e169;

            public float e170;

            public float e171;

            public float e172;

            public float e173;

            public float e174;

            public float e175;

            public float e176;

            public float e177;

            public float e178;

            public float e179;

            public float e180;

            public float e181;

            public float e182;

            public float e183;

            public float e184;

            public float e185;

            public float e186;

            public float e187;

            public float e188;

            public float e189;

            public float e190;

            public float e191;

            public float e192;

            public float e193;

            public float e194;

            public float e195;

            public float e196;

            public float e197;

            public float e198;

            public float e199;

            public float e200;

            public float e201;

            public float e202;

            public float e203;

            public float e204;

            public float e205;

            public float e206;

            public float e207;

            public float e208;

            public float e209;

            public float e210;

            public float e211;

            public float e212;

            public float e213;

            public float e214;

            public float e215;

            public float e216;

            public float e217;

            public float e218;

            public float e219;

            public float e220;

            public float e221;

            public float e222;

            public float e223;

            public float e224;

            public float e225;

            public float e226;

            public float e227;

            public float e228;

            public float e229;

            public float e230;

            public float e231;

            public float e232;

            public float e233;

            public float e234;

            public float e235;

            public float e236;

            public float e237;

            public float e238;

            public float e239;

            public float e240;

            public float e241;

            public float e242;

            public float e243;

            public float e244;

            public float e245;

            public float e246;

            public float e247;

            public float e248;

            public float e249;

            public float e250;

            public float e251;

            public float e252;

            public float e253;

            public float e254;

            public float e255;

            public float e256;

            public float e257;

            public float e258;

            public float e259;

            public float e260;

            public float e261;

            public float e262;

            public float e263;

            public float e264;

            public float e265;

            public float e266;

            public float e267;

            public float e268;

            public float e269;

            public float e270;

            public float e271;

            public float e272;

            public float e273;

            public float e274;

            public float e275;

            public float e276;

            public float e277;

            public float e278;

            public float e279;

            public float e280;

            public float e281;

            public float e282;

            public float e283;

            public float e284;

            public float e285;

            public float e286;

            public float e287;

            public float e288;

            public float e289;

            public float e290;

            public float e291;

            public float e292;

            public float e293;

            public float e294;

            public float e295;

            public float e296;

            public float e297;

            public float e298;

            public float e299;

            public float e300;

            public float e301;

            public float e302;

            public float e303;

            public float e304;

            public float e305;

            public float e306;

            public float e307;

            public float e308;

            public float e309;

            public float e310;

            public float e311;

            public float e312;

            public float e313;

            public float e314;

            public float e315;

            public float e316;

            public float e317;

            public float e318;

            public float e319;

            public float e320;

            public float e321;

            public float e322;

            public float e323;

            public float e324;

            public float e325;

            public float e326;

            public float e327;

            public float e328;

            public float e329;

            public float e330;

            public float e331;

            public float e332;

            public float e333;

            public float e334;

            public float e335;

            public float e336;

            public float e337;

            public float e338;

            public float e339;

            public float e340;

            public float e341;

            public float e342;

            public float e343;

            public float e344;

            public float e345;

            public float e346;

            public float e347;

            public float e348;

            public float e349;

            public float e350;

            public float e351;

            public float e352;

            public float e353;

            public float e354;

            public float e355;

            public float e356;

            public float e357;

            public float e358;

            public float e359;

            public float e360;

            public float e361;

            public float e362;

            public float e363;

            public float e364;

            public float e365;

            public float e366;

            public float e367;

            public float e368;

            public float e369;

            public float e370;

            public float e371;

            public float e372;

            public float e373;

            public float e374;

            public float e375;

            public float e376;

            public float e377;

            public float e378;

            public float e379;

            public float e380;

            public float e381;

            public float e382;

            public float e383;

            public float e384;

            public float e385;

            public float e386;

            public float e387;

            public float e388;

            public float e389;

            public float e390;

            public float e391;

            public float e392;

            public float e393;

            public float e394;

            public float e395;

            public float e396;

            public float e397;

            public float e398;

            public float e399;

            public float e400;

            public float e401;

            public float e402;

            public float e403;

            public float e404;

            public float e405;

            public float e406;

            public float e407;

            public float e408;

            public float e409;

            public float e410;

            public float e411;

            public float e412;

            public float e413;

            public float e414;

            public float e415;

            public float e416;

            public float e417;

            public float e418;

            public float e419;

            public float e420;

            public float e421;

            public float e422;

            public float e423;

            public float e424;

            public float e425;

            public float e426;

            public float e427;

            public float e428;

            public float e429;

            public float e430;

            public float e431;

            public float e432;

            public float e433;

            public float e434;

            public float e435;

            public float e436;

            public float e437;

            public float e438;

            public float e439;

            public float e440;

            public float e441;

            public float e442;

            public float e443;

            public float e444;

            public float e445;

            public float e446;

            public float e447;

            public float e448;

            public float e449;

            public float e450;

            public float e451;

            public float e452;

            public float e453;

            public float e454;

            public float e455;

            public float e456;

            public float e457;

            public float e458;

            public float e459;

            public float e460;

            public float e461;

            public float e462;

            public float e463;

            public float e464;

            public float e465;

            public float e466;

            public float e467;

            public float e468;

            public float e469;

            public float e470;

            public float e471;

            public float e472;

            public float e473;

            public float e474;

            public float e475;

            public float e476;

            public float e477;

            public float e478;

            public float e479;

            public float e480;

            public float e481;

            public float e482;

            public float e483;

            public float e484;

            public float e485;

            public float e486;

            public float e487;

            public float e488;

            public float e489;

            public float e490;

            public float e491;

            public float e492;

            public float e493;

            public float e494;

            public float e495;

            public float e496;

            public float e497;

            public float e498;

            public float e499;

            public float e500;

            public float e501;

            public float e502;

            public float e503;

            public float e504;

            public float e505;

            public float e506;

            public float e507;

            public float e508;

            public float e509;

            public float e510;

            public float e511;

            public float e512;

            public float e513;

            public float e514;

            public float e515;

            public float e516;

            public float e517;

            public float e518;

            public float e519;

            public float e520;

            public float e521;

            public float e522;

            public float e523;

            public float e524;

            public float e525;

            public float e526;

            public float e527;

            public float e528;

            public float e529;

            public float e530;

            public float e531;

            public float e532;

            public float e533;

            public float e534;

            public float e535;

            public float e536;

            public float e537;

            public float e538;

            public float e539;

            public float e540;

            public float e541;

            public float e542;

            public float e543;

            public float e544;

            public float e545;

            public float e546;

            public float e547;

            public float e548;

            public float e549;

            public float e550;

            public float e551;

            public float e552;

            public float e553;

            public float e554;

            public float e555;

            public float e556;

            public float e557;

            public float e558;

            public float e559;

            public float e560;

            public float e561;

            public float e562;

            public float e563;

            public float e564;

            public float e565;

            public float e566;

            public float e567;

            public float e568;

            public float e569;

            public float e570;

            public float e571;

            public float e572;

            public float e573;

            public float e574;

            public float e575;

            public float e576;

            public float e577;

            public float e578;

            public float e579;

            public float e580;

            public float e581;

            public float e582;

            public float e583;

            public float e584;

            public float e585;

            public float e586;

            public float e587;

            public float e588;

            public float e589;

            public float e590;

            public float e591;

            public float e592;

            public float e593;

            public float e594;

            public float e595;

            public float e596;

            public float e597;

            public float e598;

            public float e599;

            public float e600;

            public float e601;

            public float e602;

            public float e603;

            public float e604;

            public float e605;

            public float e606;

            public float e607;

            public float e608;

            public float e609;

            public float e610;

            public float e611;

            public float e612;

            public float e613;

            public float e614;

            public float e615;

            public float e616;

            public float e617;

            public float e618;

            public float e619;

            public float e620;

            public float e621;

            public float e622;

            public float e623;

            public float e624;

            public float e625;

            public float e626;

            public float e627;

            public float e628;

            public float e629;

            public float e630;

            public float e631;

            public float e632;

            public float e633;

            public float e634;

            public float e635;

            public float e636;

            public float e637;

            public float e638;

            public float e639;

            public float e640;

            public float e641;

            public float e642;

            public float e643;

            public float e644;

            public float e645;

            public float e646;

            public float e647;

            public float e648;

            public float e649;

            public float e650;

            public float e651;

            public float e652;

            public float e653;

            public float e654;

            public float e655;

            public float e656;

            public float e657;

            public float e658;

            public float e659;

            public float e660;

            public float e661;

            public float e662;

            public float e663;

            public float e664;

            public float e665;

            public float e666;

            public float e667;

            public float e668;

            public float e669;

            public float e670;

            public float e671;

            public float e672;

            public float e673;

            public float e674;

            public float e675;

            public float e676;

            public float e677;

            public float e678;

            public float e679;

            public float e680;

            public float e681;

            public float e682;

            public float e683;

            public float e684;

            public float e685;

            public float e686;

            public float e687;

            public float e688;

            public float e689;

            public float e690;

            public float e691;

            public float e692;

            public float e693;

            public float e694;

            public float e695;

            public float e696;

            public float e697;

            public float e698;

            public float e699;

            public float e700;

            public float e701;

            public float e702;

            public float e703;

            public float e704;

            public float e705;

            public float e706;

            public float e707;

            public float e708;

            public float e709;

            public float e710;

            public float e711;

            public float e712;

            public float e713;

            public float e714;

            public float e715;

            public float e716;

            public float e717;

            public float e718;

            public float e719;

            public float e720;

            public float e721;

            public float e722;

            public float e723;

            public float e724;

            public float e725;

            public float e726;

            public float e727;

            public float e728;

            public float e729;

            public float e730;

            public float e731;

            public float e732;

            public float e733;

            public float e734;

            public float e735;

            public float e736;

            public float e737;

            public float e738;

            public float e739;

            public float e740;

            public float e741;

            public float e742;

            public float e743;

            public float e744;

            public float e745;

            public float e746;

            public float e747;

            public float e748;

            public float e749;

            public float e750;

            public float e751;

            public float e752;

            public float e753;

            public float e754;

            public float e755;

            public float e756;

            public float e757;

            public float e758;

            public float e759;

            public float e760;

            public float e761;

            public float e762;

            public float e763;

            public float e764;

            public float e765;

            public float e766;

            public float e767;

            public float e768;

            public float e769;

            public float e770;

            public float e771;

            public float e772;

            public float e773;

            public float e774;

            public float e775;

            public float e776;

            public float e777;

            public float e778;

            public float e779;

            public float e780;

            public float e781;

            public float e782;

            public float e783;

            public float e784;

            public float e785;

            public float e786;

            public float e787;

            public float e788;

            public float e789;

            public float e790;

            public float e791;

            public float e792;

            public float e793;

            public float e794;

            public float e795;

            public float e796;

            public float e797;

            public float e798;

            public float e799;

            public float e800;

            public float e801;

            public float e802;

            public float e803;

            public float e804;

            public float e805;

            public float e806;

            public float e807;

            public float e808;

            public float e809;

            public float e810;

            public float e811;

            public float e812;

            public float e813;

            public float e814;

            public float e815;

            public float e816;

            public float e817;

            public float e818;

            public float e819;

            public float e820;

            public float e821;

            public float e822;

            public float e823;

            public float e824;

            public float e825;

            public float e826;

            public float e827;

            public float e828;

            public float e829;

            public float e830;

            public float e831;

            public float e832;

            public float e833;

            public float e834;

            public float e835;

            public float e836;

            public float e837;

            public float e838;

            public float e839;

            public float e840;

            public float e841;

            public float e842;

            public float e843;

            public float e844;

            public float e845;

            public float e846;

            public float e847;

            public float e848;

            public float e849;

            public float e850;

            public float e851;

            public float e852;

            public float e853;

            public float e854;

            public float e855;

            public float e856;

            public float e857;

            public float e858;

            public float e859;

            public float e860;

            public float e861;

            public float e862;

            public float e863;

            public float e864;

            public float e865;

            public float e866;

            public float e867;

            public float e868;

            public float e869;

            public float e870;

            public float e871;

            public float e872;

            public float e873;

            public float e874;

            public float e875;

            public float e876;

            public float e877;

            public float e878;

            public float e879;

            public float e880;

            public float e881;

            public float e882;

            public float e883;

            public float e884;

            public float e885;

            public float e886;

            public float e887;

            public float e888;

            public float e889;

            public float e890;

            public float e891;

            public float e892;

            public float e893;

            public float e894;

            public float e895;

            public float e896;

            public float e897;

            public float e898;

            public float e899;

            public float e900;

            public float e901;

            public float e902;

            public float e903;

            public float e904;

            public float e905;

            public float e906;

            public float e907;

            public float e908;

            public float e909;

            public float e910;

            public float e911;

            public float e912;

            public float e913;

            public float e914;

            public float e915;

            public float e916;

            public float e917;

            public float e918;

            public float e919;

            public float e920;

            public float e921;

            public float e922;

            public float e923;

            public float e924;

            public float e925;

            public float e926;

            public float e927;

            public float e928;

            public float e929;

            public float e930;

            public float e931;

            public float e932;

            public float e933;

            public float e934;

            public float e935;

            public float e936;

            public float e937;

            public float e938;

            public float e939;

            public float e940;

            public float e941;

            public float e942;

            public float e943;

            public float e944;

            public float e945;

            public float e946;

            public float e947;

            public float e948;

            public float e949;

            public float e950;

            public float e951;

            public float e952;

            public float e953;

            public float e954;

            public float e955;

            public float e956;

            public float e957;

            public float e958;

            public float e959;

            public float e960;

            public float e961;

            public float e962;

            public float e963;

            public float e964;

            public float e965;

            public float e966;

            public float e967;

            public float e968;

            public float e969;

            public float e970;

            public float e971;

            public float e972;

            public float e973;

            public float e974;

            public float e975;

            public float e976;

            public float e977;

            public float e978;

            public float e979;

            public float e980;

            public float e981;

            public float e982;

            public float e983;

            public float e984;

            public float e985;

            public float e986;

            public float e987;

            public float e988;

            public float e989;

            public float e990;

            public float e991;

            public float e992;

            public float e993;

            public float e994;

            public float e995;

            public float e996;

            public float e997;

            public float e998;

            public float e999;

            public float e1000;

            public float e1001;

            public float e1002;

            public float e1003;

            public float e1004;

            public float e1005;

            public float e1006;

            public float e1007;

            public float e1008;

            public float e1009;

            public float e1010;

            public float e1011;

            public float e1012;

            public float e1013;

            public float e1014;

            public float e1015;

            public float e1016;

            public float e1017;

            public float e1018;

            public float e1019;

            public float e1020;

            public float e1021;

            public float e1022;

            public float e1023;

            public float e1024;
            #endregion

            #region Properties
            public float this[int index]
            {
                get
                {
                    if ((uint)(index) > 1024) // (index < 0) || (index > 1024)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (float* e = &e0)
                    {
                        return e[index];
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
