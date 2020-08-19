/* ----- Economy Tweak mod by heu3becteh, version 1.141.4 -----
 * 
 * While I like economy the in game, there is still room for improvements to realize its potential. I have tried to make economy in the game even more realistic, logical and interesting.
 * 
 * ---------- List of changes this mod introduces ----------
 * 
 * - Demand needs to be fulfilled. In the unmodified game unfulfilled demand is just disregarded. With this mod it affects prosperity.
 * - Workshops demand is explicitly taken into account during demand calculation. In the unmodified game demand is defined by prosperity. Since e1.4.1 some changes were done, but I think that workshops demand is better treated explicitly.
 * - Prosperity and Wealth (Gold) are linked and could be converted from one to another, no hardcoded artificial town gold spawn-despawn. In the unmodified game town gold is being spawn-despawn depending on prosperity to get closer to gold = prosperity * 7, 20% per day.
 * - When there is a food shortage, prosperity is decreased before everyone starves to death. In the unmodified game only starvation and death decrease prosperity. With this mod enabled food shortage is additionally increasing the food demand.
 * - Stocks of everything are increased (raw resource production is increased on 50%, basic demand is halved, equipment demand is increased), with this mod enabled the stocks are generally divided into more or less, and not into absent or present.
 * - More caravans to transfer increased stock amounts.
 * - Towns take into account optimal stock amount for the demand (10 days worth of demand fulfillment), the price gets higher than default value if the current stock amount less than that, lower if the current stock amount is greater.
 * Towns consume goods faster when there is a surplus of specific category goods.
 * Simple logic of price calculation: basic equation is (optimal stock amount + demand + 1) / (actual stock amount + demand + 1), it determines whether price multiplier will be > 1 or < 1. Price multiplier is divided into short-term (calculated for each item purchase taking into account only the current market situation) and long-term (change calculated daily, has some inertia). The same pretty much applies to the unmodified game, but the basic equation is demand / (0.1f * supply + actual stock amount * 0.04f + 2f), where supply is long-term component and actual stock amount is short-term component. There is no particular advantage of the equation used in this mod, but you can note that long-term component is more significant than short-term component. In this mod to disctribute goods better across the map short-term component is more significant.
 * - Daily changes use another equation, compared to the unmodified game. In the unmodified game inertia is introduced with new = old * 0.85 + new * 0.15 equation. It has a downside that value increases faster than decreases. Because of that if you will sell large (the larger the more significant that effect is) amounts of trade good to town, wait for daily changes to take place and then buy it back, inertia equation could increase the value significantly in one day, but than decrease it linearly on 15% maximum. And nobody will sell these goods for low price to the town that has high demand for it really. Caravans can even buy food from a starving town because it is cheap.
 * With this mod inertia equation is changed to new = old * (new / old)^0.2, with that equation value can decrease faster depending on the change significance, and it does not increase as fast.
 * 
 * ---------------------- Discussion -----------------------
 * 
 * Generally with this mod enabled prosperity changes faster, which makes economy more dynamic. But, while fluctuations are faster, they are approaching equilibrium state, which could be stable enough, but not static.
 * While some conceptual changes may be controversial, this mod addresses some issues that should be addressed in the unmodified game, that way or another.
 * 
 * With this mod economy is alive and can balance itself, without need for hardcoded caps on everything and spawn-despawn of things when situation goes wrong. That pretty much goes for the unmodified game too, this mod does not change core aspect of that, but removes some limits and introduces some new mechanics / applies fixes that could be considered.
 * I would like to see the economy in the game where everything converst from one form to another, without disappearing and appearing out of nowhere. Sources of wealth would be villages producing raw goods and added value of workshop production, sinks would be destructive actions of men, unfulfilled demands, starvation.
 * This mod aspires to get closer to that. Maybe without limitations and spawn-despawn of neccessary things something can be broken, but I do not see a way for that to happen (unless there is kinda bug) and did not experience anything of sort during tests. Things are balanced and if one aspect goes wrong, it will be fixed by other.
 * 
 * While economy gets better and better with patches and has great potential to be interesting without any mod, I hope some solutions from modifications could help to make the unmodified game batter too during development.
 * 
 * ------------------- Save Compability --------------------
 * 
 * This mod does not create any additional save data (which makes some things to go different with repeated save-loads), so it should not corrupt the saves.
 * Some time is required after this mod is enabled for economy to stabilize. This mod changes supply-demand-prosperity, because of that some time will be required after that mod is disabled for things to get stable again without the mod.
 * 
 * -------------------- Mod Compability --------------------
 * 
 * This mod is created with e1.4.1 (current) game version in mind. It could not work as intended with other game versions.
 * Version this mod was build for is indicated in this mod version: 1.*game version*.*mod version for the specific game version*.
 * If you have installed this mod with version not compatible, it should not cause major problems (it does not change much and I have tried to add some safety measures). For some cases before I have added this measures, DisableSupplyDemandInertia submod should completely remove all the persistent changes this mod could've caused.
 * Version 1.4.1 did introduce GetEstimatedDemandForCategory(), which is not good for this mod and protected (so it is hard to prevent it from running as it is). Because of that it has to run, but after that GetEstimatedDemandForCategory() changes to demand are nullified in other method GetSupplyDemandForCategory(). So in versions prior to that change it introduces some other unintended change instead. It could be not critical, but has a good chance to cause bugs.
 * 
 * This mod could conflict when other mod replaces (not postfix/prefix) the same methods, which are listed below.
 * It could conflict with mods dependant on supply variable.
 * Other from that, it should be pretty much compatible with anything.
 * 
 * - This mod changes methods to estimate supply and demand, but general mechanisms are the same as in the unmodified game, results look similar, except for supply variable, which is town item category stock amount/value with inertia. In this mod that is replaced by multiplier, because when it is called by GetBasePriceFactor() there is no town data indicated.
 * This mod should not cause problems to pretty much any mod, which do not use supply variable.
 * This mod will have problems if supply-demand variables will be used to store some other data before it is called.
 * Changed methods: GetDailyDemandForCategory() replaced, GetSupplyDemandForCategory() replaced, GetBasePriceFactor() replaced, MakeConsumption() replaced, some values are transferred by dictionary to CalculateProsperityChange().
 * - This mod adds prosperity changes after default as Postfix to have prosperity value more dynamic.
 * As it is Postfix it should not affect anything, should not cause problems to any mod.
 * This mod will have problems if prosperity-gold variables will be used to store some other data before it is called.
 * Changed methods: CalculateProsperityChange() postfix, GetTownGoldChange() replaced.
 * - This mod adds 50% more production to vilages. CalculateDailyProductionAmount() postfix, which multiplies result by ProductionMultiplier = 1.5.
 * As it is Postfix it should not affect anything, should not cause problems to any mod.
 * This mod will have problems if production variables will be used to store some other data before it is called.
 * Changed methods: CalculateDailyProductionAmount() postfix.
 * - This mod allows heroes to have up to 2 caravans.
 * Should not cause problems, it only makes it possible for SpawnCaravans() to return true when hero.OwnedCaravans.Count<MobileParty>() = 1.
 * Changed methods: SpawnCaravans() replaced.
 * 
 * --------------------- Installation ----------------------
 * 
 * Unpack contents of zip-archive to "...\Mount & Blade II Bannerlord\Modules\".
 * 
 * ----------------------- Settings ------------------------
 * 
 * You can change the mod settings in "...\Mount & Blade II Bannerlord\Modules\EconomyTweak_h\config.xml" file.
 * 
 * ------------------------- Links -------------------------
 * 
 * 
 * Topic with the discussion of suggestions regarding economy:
 * https://forums.taleworlds.com/index.php?threads/supply-and-demand-self-balancing-economy-and-how-it-does-work-now.414875/
 * Mod at TaleWorlds forum:
 * https://forums.taleworlds.com/index.php?threads/economy-tweak.425829/
 * Mod at Nexus:
 * https://www.nexusmods.com/mountandblade2bannerlord/mods/1828
 * 
 * 22.06.2020 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.IO; // To write log-files.
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace EconomyTweak_h
{
	public class SubModule : MBSubModuleBase
	{
		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();

			EconomyTweak_h_globalConstants.EconomyTweak_h_globalConstants_read();

			var harmony = new Harmony("EconomyTweak_h");
			harmony.PatchAll();
		}
		protected override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			base.OnBeforeInitialModuleScreenSetAsRoot();
			InformationManager.DisplayMessage(new InformationMessage("EconomyTweak by heu3becteh v1.141.4"));
		}
	}
	public class EconomyTweak_h_globalConstants
	{
		public static int EconomyTweak_h_OptimalStockPeriodPrice; // Period in days used to calculate prices: towns will have prices below item value if the current stock is more than enough to fulfill demands for that period, above item value if not enough.
		public static float EconomyTweak_h_LongtermPriceMultiplierExpValue; // Long-term price multiplier will be exponentiated by MathF.Pow with that value.
		public static float EconomyTweak_h_ShorttermPriceMultiplierExpValue; // Short-term price multiplier will be exponentiated by MathF.Pow with that value.
		public static float EconomyTweak_h_ProsperityPriceFactorExpValue; // Prices in towns with < 3000 prosperity are slightly increased, with > 3000 prosperity slightly decreased, multiplier will be exponentiated by MathF.Pow with that value.
		public static int EconomyTweak_h_OptimalStockPeriodOverconsumption; // Period in days used to calculate overconsumption: towns will faster consume stocks above amount needed for that period.
		public static float EconomyTweak_h_DemandMultiplier; // Demand is multiplied by this value.
		public static float EconomyTweak_h_EquipmentDemandMultiplier; // Equipment demand is additionally multiplied by this value.
		public static float EconomyTweak_h_ProductionMultiplier; // Production is multiplied by that value.
		public static int EconomyTweak_h_OptimalStockPeriodFood; // Towns will be more wary of food shortages when they have not enough food for that period.
		public static float EconomyTweak_h_FoodShortageProsperityExpValue; // Prosperity change due to food shortage will be exponentiated by MathF.Pow with that value.
		public static float EconomyTweak_h_TownDemandFulfilledProsperityExpValue; // Prosperity change due to demands fulfilled will be exponentiated by MathF.Pow with that value.
		public static float EconomyTweak_h_ValueOfProsperity; // 1 prosperity = 1 gold * EconomyTweak_h_ValueOfProsperity
		public static int EconomyTweak_h_DebugLevel; // Defines debug level: how much will be written to 'EconomyTweak_h_log.txt'.
		public static bool EconomyTweak_h_DebugDisplay; // Defines debug level: when true, info about methods to run will be displayed in game.
		public static string EconomyTweak_h_path = BasePath.Name + "Modules/EconomyTweak_h/";
		public static void EconomyTweak_h_globalConstants_read()
        {
			// Default settings.
			EconomyTweak_h_Configuration EconomyTweak_h_config_init = new EconomyTweak_h_Configuration();
			EconomyTweak_h_config_init.EconomyTweak_h_OptimalStockPeriodPrice = 30;
			EconomyTweak_h_config_init.EconomyTweak_h_LongtermPriceMultiplierExpValue = 0.1f;
			EconomyTweak_h_config_init.EconomyTweak_h_ShorttermPriceMultiplierExpValue = 0.5f;
			EconomyTweak_h_config_init.EconomyTweak_h_ProsperityPriceFactorExpValue = 0.2f;
			EconomyTweak_h_config_init.EconomyTweak_h_OptimalStockPeriodOverconsumption = 15;
			EconomyTweak_h_config_init.EconomyTweak_h_DemandMultiplier = 0.5f;
			EconomyTweak_h_config_init.EconomyTweak_h_EquipmentDemandMultiplier = 4f;
			EconomyTweak_h_config_init.EconomyTweak_h_ProductionMultiplier = 1.5f;
			EconomyTweak_h_config_init.EconomyTweak_h_OptimalStockPeriodFood = 15;
			EconomyTweak_h_config_init.EconomyTweak_h_FoodShortageProsperityExpValue = 0.5f;
			EconomyTweak_h_config_init.EconomyTweak_h_TownDemandFulfilledProsperityExpValue = 0.5f;
			EconomyTweak_h_config_init.EconomyTweak_h_ValueOfProsperity = 100f;
			EconomyTweak_h_config_init.EconomyTweak_h_DebugLevel = 0;
			EconomyTweak_h_config_init.EconomyTweak_h_DebugDisplay = true;
			// Save default settings to a file "config_default.xml".
			EconomyTweak_h_Configuration.Serialize(EconomyTweak_h_path + "config_default.xml", EconomyTweak_h_config_init);
			using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_path + "config_default.xml"))
			{
				Economy_log_writer_WriteText.WriteLine("\r\n<!-- Description of constants:\r\n" +
					"EconomyTweak_h_OptimalStockPeriodPrice - Period in days used to calculate prices: towns will have prices below item value if the current stock is more than enough to fulfill demands for that period, above item value if not enough;\r\n" +
					"EconomyTweak_h_LongtermPriceMultiplierExpValue - Long-term price multiplier will be exponentiated by MathF.Pow with that value;\r\n" +
					"EconomyTweak_h_ShorttermPriceMultiplierExpValue - Short-term price multiplier will be exponentiated by MathF.Pow with that value;\r\n" +
					"EconomyTweak_h_ProsperityPriceFactorExpValue - Prices in towns with < 3000 prosperity are slightly increased, with > 3000 prosperity slightly decreased, multiplier will be exponentiated by MathF.Pow with that value;\r\n" +
					"EconomyTweak_h_OptimalStockPeriodOverconsumption - Period in days used to calculate overconsumption: towns will faster consume stocks above amount needed for that period;\r\n" +
					"EconomyTweak_h_DemandMultiplier - Demand is multiplied by this value;\r\n" +
					"EconomyTweak_h_EquipmentDemandMultiplier - Equipment demand is additionally multiplied by this value;\r\n" +
					"EconomyTweak_h_ProductionMultiplier - Production is multiplied by that value;\r\n" +
					"EconomyTweak_h_OptimalStockPeriodFood - Towns will be more wary of food shortages when they have not enough food for that period;\r\n" +
					"EconomyTweak_h_FoodShortageProsperityExpValue - Prosperity change due to food shortage will be exponentiated by MathF.Pow with that value;\r\n" +
					"EconomyTweak_h_TownDemandFulfilledProsperityExpValue - Prosperity change due to demands fulfilled will be exponentiated by MathF.Pow with that value;\r\n" +
					"EconomyTweak_h_ValueOfProsperity - 1 prosperity = 1 gold * EconomyTweak_h_ValueOfProsperity;\r\n" +
					"EconomyTweak_h_DebugLevel - Defines debug level: how much will be written to 'EconomyTweak_h_log.txt';\r\n" +
					"EconomyTweak_h_DebugDisplay - Defines debug level: when true, info about methods to run will be displayed in game.\r\n" +
					"Default values in 'config_default.xml' file. -->");
			}
			// Read settings from "config.xml".
			EconomyTweak_h_Configuration EconomyTweak_h_config;
			if (File.Exists(EconomyTweak_h_path + "config.xml"))
			{
				using (StreamWriter Economy_log_writer_WriteText = File.CreateText(EconomyTweak_h_path + "EconomyTweak_h_log.txt"))
				{
					Economy_log_writer_WriteText.WriteLine("Settings are loaded from the file '" + EconomyTweak_h_path + "config.xml'");
				}
				EconomyTweak_h_config = EconomyTweak_h_Configuration.Deserialize(EconomyTweak_h_path + "config.xml");
            }
            else
            {
				using (StreamWriter Economy_log_writer_WriteText = File.CreateText(EconomyTweak_h_path + "EconomyTweak_h_log.txt"))
				{
					Economy_log_writer_WriteText.WriteLine("File '" + EconomyTweak_h_path + "config.xml'" + " does not exist. Using the default settings from '" + EconomyTweak_h_path + "config_default.xml'.");
				}
				EconomyTweak_h_config = EconomyTweak_h_Configuration.Deserialize(EconomyTweak_h_path + "config_default.xml");
			}
			// Write out the variables read from the file
			EconomyTweak_h_OptimalStockPeriodPrice = EconomyTweak_h_config.EconomyTweak_h_OptimalStockPeriodPrice;
			EconomyTweak_h_LongtermPriceMultiplierExpValue = EconomyTweak_h_config.EconomyTweak_h_LongtermPriceMultiplierExpValue;
			EconomyTweak_h_ShorttermPriceMultiplierExpValue = EconomyTweak_h_config.EconomyTweak_h_ShorttermPriceMultiplierExpValue;
			EconomyTweak_h_ProsperityPriceFactorExpValue = EconomyTweak_h_config.EconomyTweak_h_ProsperityPriceFactorExpValue;
			EconomyTweak_h_OptimalStockPeriodOverconsumption = EconomyTweak_h_config.EconomyTweak_h_OptimalStockPeriodOverconsumption;
			EconomyTweak_h_DemandMultiplier = EconomyTweak_h_config.EconomyTweak_h_DemandMultiplier;
			EconomyTweak_h_EquipmentDemandMultiplier = EconomyTweak_h_config.EconomyTweak_h_EquipmentDemandMultiplier;
			EconomyTweak_h_ProductionMultiplier = EconomyTweak_h_config.EconomyTweak_h_ProductionMultiplier;
			EconomyTweak_h_OptimalStockPeriodFood = EconomyTweak_h_config.EconomyTweak_h_OptimalStockPeriodFood;
			EconomyTweak_h_FoodShortageProsperityExpValue = EconomyTweak_h_config.EconomyTweak_h_FoodShortageProsperityExpValue;
			EconomyTweak_h_TownDemandFulfilledProsperityExpValue = EconomyTweak_h_config.EconomyTweak_h_TownDemandFulfilledProsperityExpValue;
			EconomyTweak_h_ValueOfProsperity = EconomyTweak_h_config.EconomyTweak_h_ValueOfProsperity;
			EconomyTweak_h_DebugLevel = EconomyTweak_h_config.EconomyTweak_h_DebugLevel;
			EconomyTweak_h_DebugDisplay = EconomyTweak_h_config.EconomyTweak_h_DebugDisplay;
			// Write the current settings to log.
			if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugDisplay) { InformationManager.DisplayMessage(new InformationMessage("EconomyTweak_h_globalConstants_read")); }
			using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt"))
			{
				Economy_log_writer_WriteText.WriteLine("EconomyTweak settings loaded:\r\n" +
					"EconomyTweak_h_OptimalStockPeriodPrice = " + EconomyTweak_h_globalConstants.EconomyTweak_h_OptimalStockPeriodPrice.ToString() +
					", EconomyTweak_h_LongtermPriceMultiplierExpValue = " + EconomyTweak_h_globalConstants.EconomyTweak_h_LongtermPriceMultiplierExpValue.ToString() +
					", EconomyTweak_h_ShorttermPriceMultiplierExpValue = " + EconomyTweak_h_globalConstants.EconomyTweak_h_ShorttermPriceMultiplierExpValue.ToString() +
					", EconomyTweak_h_ProsperityPriceFactorExpValue = " + EconomyTweak_h_globalConstants.EconomyTweak_h_ProsperityPriceFactorExpValue.ToString() +
					", EconomyTweak_h_OptimalStockPeriodOverconsumption = " + EconomyTweak_h_globalConstants.EconomyTweak_h_OptimalStockPeriodOverconsumption.ToString() +
					", EconomyTweak_h_DemandMultiplier = " + EconomyTweak_h_globalConstants.EconomyTweak_h_DemandMultiplier.ToString() +
					", EconomyTweak_h_EquipmentDemandMultiplier = " + EconomyTweak_h_globalConstants.EconomyTweak_h_EquipmentDemandMultiplier.ToString() +
					", EconomyTweak_h_ProductionMultiplier = " + EconomyTweak_h_globalConstants.EconomyTweak_h_ProductionMultiplier.ToString() +
					", EconomyTweak_h_OptimalStockPeriodFood = " + EconomyTweak_h_globalConstants.EconomyTweak_h_OptimalStockPeriodFood.ToString() +
					", EconomyTweak_h_FoodShortageProsperityExpValue = " + EconomyTweak_h_globalConstants.EconomyTweak_h_FoodShortageProsperityExpValue.ToString() +
					", EconomyTweak_h_TownDemandFulfilledProsperityExpValue = " + EconomyTweak_h_globalConstants.EconomyTweak_h_TownDemandFulfilledProsperityExpValue.ToString() +
					", EconomyTweak_h_ValueOfProsperity = " + EconomyTweak_h_globalConstants.EconomyTweak_h_ValueOfProsperity.ToString() +
					", EconomyTweak_h_DebugLevel = " + EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel.ToString() +
					", EconomyTweak_h_DebugDisplay = " + EconomyTweak_h_globalConstants.EconomyTweak_h_DebugDisplay.ToString());
			}
		}
	}
	public class EconomyTweak_h_Configuration
	// Used to read EconomyTweak_h_globalConstants from "config.xml".
	{
		public static int _EconomyTweak_h_OptimalStockPeriodPrice;
		public static float _EconomyTweak_h_LongtermPriceMultiplierExpValue;
		public static float _EconomyTweak_h_ShorttermPriceMultiplierExpValue;
		public static float _EconomyTweak_h_ProsperityPriceFactorExpValue;
		public static int _EconomyTweak_h_OptimalStockPeriodOverconsumption;
		public static float _EconomyTweak_h_DemandMultiplier;
		public static float _EconomyTweak_h_EquipmentDemandMultiplier;
		public static float _EconomyTweak_h_ProductionMultiplier;
		public static int _EconomyTweak_h_OptimalStockPeriodFood;
		public static float _EconomyTweak_h_FoodShortageProsperityExpValue;
		public static float _EconomyTweak_h_TownDemandFulfilledProsperityExpValue;
		public static float _EconomyTweak_h_ValueOfProsperity;
		public static int _EconomyTweak_h_DebugLevel;
		public static bool _EconomyTweak_h_DebugDisplay;
		public EconomyTweak_h_Configuration()
		{
			_EconomyTweak_h_DebugLevel = 0;
			_EconomyTweak_h_DebugDisplay = true;
		}
		public static void Serialize(string file, EconomyTweak_h_Configuration EconomyTweak_h_config)
		{
			System.Xml.Serialization.XmlSerializer xs
			   = new System.Xml.Serialization.XmlSerializer(EconomyTweak_h_config.GetType());
			StreamWriter writer = File.CreateText(file);
			xs.Serialize(writer, EconomyTweak_h_config);
			writer.Flush();
			writer.Close();
		}
		public static EconomyTweak_h_Configuration Deserialize(string file)
		{
			System.Xml.Serialization.XmlSerializer xs
			   = new System.Xml.Serialization.XmlSerializer(
				  typeof(EconomyTweak_h_Configuration));
			StreamReader reader = File.OpenText(file);
			EconomyTweak_h_Configuration EconomyTweak_h_config_init = (EconomyTweak_h_Configuration)xs.Deserialize(reader);
			reader.Close();
			return EconomyTweak_h_config_init;
		}
		public int EconomyTweak_h_OptimalStockPeriodPrice
		{
			get { return _EconomyTweak_h_OptimalStockPeriodPrice; }
			set { _EconomyTweak_h_OptimalStockPeriodPrice = value; }
		}
		public float EconomyTweak_h_LongtermPriceMultiplierExpValue
		{
			get { return _EconomyTweak_h_LongtermPriceMultiplierExpValue; }
			set { _EconomyTweak_h_LongtermPriceMultiplierExpValue = value; }
		}
		public float EconomyTweak_h_ShorttermPriceMultiplierExpValue
		{
			get { return _EconomyTweak_h_ShorttermPriceMultiplierExpValue; }
			set { _EconomyTweak_h_ShorttermPriceMultiplierExpValue = value; }
		}
		public float EconomyTweak_h_ProsperityPriceFactorExpValue
		{
			get { return _EconomyTweak_h_ProsperityPriceFactorExpValue; }
			set { _EconomyTweak_h_ProsperityPriceFactorExpValue = value; }
		}
		public int EconomyTweak_h_OptimalStockPeriodOverconsumption
		{
			get { return _EconomyTweak_h_OptimalStockPeriodOverconsumption; }
			set { _EconomyTweak_h_OptimalStockPeriodOverconsumption = value; }
		}
		public float EconomyTweak_h_DemandMultiplier
		{
			get { return _EconomyTweak_h_DemandMultiplier; }
			set { _EconomyTweak_h_DemandMultiplier = value; }
		}
		public float EconomyTweak_h_EquipmentDemandMultiplier
		{
			get { return _EconomyTweak_h_EquipmentDemandMultiplier; }
			set { _EconomyTweak_h_EquipmentDemandMultiplier = value; }
		}
		public float EconomyTweak_h_ProductionMultiplier
		{
			get { return _EconomyTweak_h_ProductionMultiplier; }
			set { _EconomyTweak_h_ProductionMultiplier = value; }
		}
		public int EconomyTweak_h_OptimalStockPeriodFood
		{
			get { return _EconomyTweak_h_OptimalStockPeriodFood; }
			set { _EconomyTweak_h_OptimalStockPeriodFood = value; }
		}
		public float EconomyTweak_h_FoodShortageProsperityExpValue
		{
			get { return _EconomyTweak_h_FoodShortageProsperityExpValue; }
			set { _EconomyTweak_h_FoodShortageProsperityExpValue = value; }
		}
		public float EconomyTweak_h_TownDemandFulfilledProsperityExpValue
		{
			get { return _EconomyTweak_h_TownDemandFulfilledProsperityExpValue; }
			set { _EconomyTweak_h_TownDemandFulfilledProsperityExpValue = value; }
		}
		public float EconomyTweak_h_ValueOfProsperity
		{
			get { return _EconomyTweak_h_ValueOfProsperity; }
			set { _EconomyTweak_h_ValueOfProsperity = value; }
		}
		public int EconomyTweak_h_DebugLevel
		{
			get { return _EconomyTweak_h_DebugLevel; }
			set { _EconomyTweak_h_DebugLevel = value; }
		}
		public bool EconomyTweak_h_DebugDisplay
		{
			get { return _EconomyTweak_h_DebugDisplay; }
			set { _EconomyTweak_h_DebugDisplay = value; }
		}
	}
	public class EconomyTweak_h_Dictionaries
	{
		// I wanted to initialize dictionaries with "foreach (Town townFromAll in Town.AllTowns)", but Town.AllTowns is defined too late, it seems. So checks for dictionary.ContainsKey(town) were done at some places.
		public static Dictionary<(Town, ItemCategory), float> EconomyTweak_h_TownCategoryWorkshopPriceFactorDictionary = new Dictionary<(Town, ItemCategory), float>(); // Multiplier to take into account workshop demands during price calculation.
		public static Dictionary<(Town, ItemCategory), float> EconomyTweak_h_TownCategoryProsperityPriceFactorDictionary = new Dictionary<(Town, ItemCategory), float>(); // Multiplier to slightly reduce prosperity impact during price calculation.
		public static Dictionary<Town, float> EconomyTweak_h_TownDemandFulfilledDictionary = new Dictionary<Town, float>(); // Dictionary to remember which demands were fulfilled and which were not, and to take that into account during prosperity change calculation.
		public static void EconomyTweak_h_TownDemandFulfilledAdd(Town town, float demandFulfilled)
		{
			if (EconomyTweak_h_Dictionaries.EconomyTweak_h_TownDemandFulfilledDictionary.ContainsKey(town))
			{
				EconomyTweak_h_TownDemandFulfilledDictionary[town] += demandFulfilled;
			}
			else
			{
				EconomyTweak_h_TownDemandFulfilledDictionary[town] = demandFulfilled;
			}
		}
	}

	[HarmonyPatch(typeof(DefaultSettlementEconomyModel), "GetDailyDemandForCategory")]
	internal class EconomyTweak_h_GetDailyDemandForCategory_patch
	{
		// Method to calculate the most basic demand, from which daily demand all prices and consumption are calculated.
		// It depends linearly on prosperity originally, as simple as it gets.
		// EconomyTweak mod halves the value of that demand.
		// EconomyTweak mod also takes into account workshops, but that part of demand is used only for price calculation (for caravans to bring raw materials to workshop), it is substracted when making consumption.
		// It could be done separately from demand, but that would require to have additional parameters (town) when method to calculate price is called (it uses demand, supply and inStoreValue for itemCategory, check "EconomyTweak_h_GetBasePriceFactor()").
		// EconomyTweak mod also adds multiplier to slightly decrease influence of prosperity on price, originally prices are considerably higher in towns with higher prosperity.
		// EconomyTweak mod also increases food demand during food shortages.
		[HarmonyPrefix]
		public static bool EconomyTweak_h_GetDailyDemandForCategory(Town town, ItemCategory category, ref float __result)
		{
			float EconomyTweak_h_workShopProduction = 0;
			for (int i_TownWorkshop = 0; i_TownWorkshop < town.Workshops.Length; i_TownWorkshop++)
			{
				for (int i_production = 0; i_production < town.Workshops[i_TownWorkshop].WorkshopType.Productions.Count; i_production++)
                {
					IEnumerable<ValueTuple<ItemCategory, int>> inputs = town.Workshops[i_TownWorkshop].WorkshopType.Productions[i_production].Inputs;
					foreach (ValueTuple<ItemCategory, int> valueTuple in inputs)
					{
						if (category == valueTuple.Item1)
						{
							float EconomyTweak_h_categoryAverageValue = category.AverageValue;
							// Demands are expressed in value, not in item quantities, so ConversionSpeed is divided by itemObject.Value.
							EconomyTweak_h_workShopProduction = EconomyTweak_h_workShopProduction + town.Workshops[i_TownWorkshop].WorkshopType.Productions[i_production].ConversionSpeed / EconomyTweak_h_categoryAverageValue;
						}
					}
				}
			}
			float num = Math.Max(1f, town.Prosperity);
			float num2 = Math.Max(1f, num - 3000f);
			float num3 = category.BaseDemand * EconomyTweak_h_globalConstants.EconomyTweak_h_DemandMultiplier * num; // EconomyTweak_h_DemandMultiplier is added to original equation.
			float num4 = category.LuxuryDemand * EconomyTweak_h_globalConstants.EconomyTweak_h_DemandMultiplier * num2; // EconomyTweak_h_DemandMultiplier is added to original equation.
			float result = num3 + num4;
			if (category.BaseDemand < 1E-08f)
			{
				result = num * 0.01f;
			}
			// Workshops ConversionSpeed and town Prosperity are taken into account by multipliers stored in dictionaries. These multipliers are removed when calculating consumption.
			EconomyTweak_h_Dictionaries.EconomyTweak_h_TownCategoryWorkshopPriceFactorDictionary[(town, category)] = MathF.Clamp(1 + EconomyTweak_h_workShopProduction / (category.BaseDemand / 2 + category.LuxuryDemand / 2 + EconomyTweak_h_workShopProduction), 0.5f, 1.5f);
			result = result * EconomyTweak_h_Dictionaries.EconomyTweak_h_TownCategoryWorkshopPriceFactorDictionary[(town, category)];
			EconomyTweak_h_Dictionaries.EconomyTweak_h_TownCategoryProsperityPriceFactorDictionary[(town, category)] = MathF.Clamp(MathF.Pow(3000 / (town.Prosperity + 1), EconomyTweak_h_globalConstants.EconomyTweak_h_ProsperityPriceFactorExpValue), 0.5f, 1.5f);
			result = result * EconomyTweak_h_Dictionaries.EconomyTweak_h_TownCategoryProsperityPriceFactorDictionary[(town, category)];
			// During food shortage demand on food (ItemCategory.Property.BonusToFoodStores) is increased.
			float EconomyTweak_h_foodShortageDemand = 0;
			// Towns will aspire to have stocks more than Prosperity / 50 + FoodChange * OptimalStockPeriodFood.
			float EconomyTweak_h_FoodDeficiency = town.FoodStocks + town.FoodChange * EconomyTweak_h_globalConstants.EconomyTweak_h_OptimalStockPeriodFood - town.Prosperity / 50;
			if (EconomyTweak_h_FoodDeficiency < 0)
			{
				if (category.Properties == ItemCategory.Property.BonusToFoodStores)
                {
					// Maximum change of demand due to food shortage is 10% of demand. It will accumulate if food shortage continues, but should not change demand in one day too much.
					EconomyTweak_h_foodShortageDemand = Math.Min(-EconomyTweak_h_FoodDeficiency, result * 0.1f);
					result = result + EconomyTweak_h_foodShortageDemand;
				}
            }
			// Equipment demand is not enough to make players who rely on loot have enough income. To increase prices of equipment and decrease large stocks of equipment at towns, equipment demand is increased.
			if (!category.IsTradeGood)
			{
				result = result * EconomyTweak_h_globalConstants.EconomyTweak_h_EquipmentDemandMultiplier;
			}
			__result = result;
			if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 0)
			{
				using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt")) { Economy_log_writer_WriteText.WriteLine("EconomyTweak_h_GetDailyDemandForCategory"); }
				if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugDisplay) { InformationManager.DisplayMessage(new InformationMessage("EconomyTweak_h_GetDailyDemandForCategory")); }
				if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 1)
                {
					using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt"))
					{
						Economy_log_writer_WriteText.WriteLine("town = " + town.ToString() +
							", category = " + category.ToString() +
							", EconomyTweak_h_workShopProduction = " + EconomyTweak_h_workShopProduction.ToString() +
							", BaseDemand(num3) = " + num3.ToString() +
							", LuxuryDemand(num4) = " + num4.ToString() +
							", EconomyTweak_h_TownCategoryWorkshopPriceFactorDictionary = " + EconomyTweak_h_Dictionaries.EconomyTweak_h_TownCategoryWorkshopPriceFactorDictionary[(town, category)].ToString() +
							", EconomyTweak_h_TownCategoryProsperityPriceFactorDictionary = " + EconomyTweak_h_Dictionaries.EconomyTweak_h_TownCategoryProsperityPriceFactorDictionary[(town, category)].ToString() +
							", EconomyTweak_h_foodShortageDemand = " + EconomyTweak_h_foodShortageDemand.ToString() +
							", category.IsTradeGood = " + category.IsTradeGood.ToString() +
							", EconomyTweak_h_GetDailyDemandForCategory(result) = " + __result.ToString());
					}
				}
			}
			return false; // Do not call the original method.
		}
	}

	[HarmonyPatch(typeof(DefaultSettlementEconomyModel), "GetSupplyDemandForCategory")]
	internal class EconomyTweak_h_GetSupplyDemandForCategory_patch
	{
		// Method to calculate daily demand.
		// It takes old demand and demand from EconomyTweak_h_GetDailyDemandForCategory() to calculate new daily demand. Demand changes from old to new with some inertia.
		// EconomyTweak mod changes the equation to use EconomyTweak_h_globalConstants.EconomyTweak_h_OptimalStockPeriod and do not use supply. Supply estimation is complicated and not intuitive in original and is replaced by actual stock value (town.MarketData.GetItemCountOfCategory).
		// Instead of original supply, LongtermPriceMultiplier is stored in supply value. Because supply in original is much higher than that multiplier, prices will be higher during the first time after the EconomyTweak mod is disabled.
		[HarmonyPrefix]
		public static bool EconomyTweak_h_GetSupplyDemandForCategory(Town town, ItemCategory category, float dailySupply, float dailyDemand, float oldSupply, float oldDemand, ref ValueTuple<float, float> __result)
		{
			// GetEstimatedDemandForCategory() subtracts some value related to stock amount from demand. While it does help to indicate real demand and account for workshops, it conflicts with EconomyTweak mod (supply is used to store long-term price multiplier), where workshops are taken into account explicitly.
			// It is problematic to edit overprotected GetEstimatedDemandForCategory, so that addition should be removed after it is done.
			float EconomyTweak_h_estimatedDemandForCategoryDemandOrig = (dailyDemand - 0.1f * oldSupply + 0.03f * dailySupply) / 0.9f;
			if (dailySupply - EconomyTweak_h_estimatedDemandForCategoryDemandOrig - 0.03f * dailySupply > 0)
			{
				if (EconomyTweak_h_estimatedDemandForCategoryDemandOrig > 0)
				{
					dailyDemand = EconomyTweak_h_estimatedDemandForCategoryDemandOrig;
				}
				else
				{
					InformationManager.DisplayMessage(new InformationMessage("EconomyTweak: GetEstimatedDemandForCategory() problem")); // Most probably the current game version is not compatible.
				}
			}
			float EconomyTweak_h_StoreValue = dailySupply;
			float EconomyTweak_h_OptimalStockValue = EconomyTweak_h_globalConstants.EconomyTweak_h_OptimalStockPeriodPrice * dailyDemand;
			float item = oldDemand * MathF.Pow((dailyDemand / oldDemand), 0.2f);
			// EconomyTweak_h_LongtermPriceMultiplier calculated, together with old EconomyTweak_h_LongtermPriceMultiplier it will form new EconomyTweak_h_LongtermPriceMultiplier.
			// ((StoreValue+dailyDemand+1)/(OptimalStockValue+dailyDemand+1))^0.1
			// 0.2 exponentiation brings EconomyTweak_h_LongtermPriceMultiplier closer to 1, increasing the relative impact of short-term part of BasePriceFactor, which in other aspects is canlculated with the same equation.
			float EconomyTweak_h_LongtermPriceMultiplier = MathF.Pow((EconomyTweak_h_OptimalStockValue + dailyDemand + 1)/(EconomyTweak_h_StoreValue + dailyDemand + 1), EconomyTweak_h_globalConstants.EconomyTweak_h_LongtermPriceMultiplierExpValue);
			// New EconomyTweak_h_LongtermPriceMultiplier is stored in num (new supply value).
			if (oldSupply > 10) { oldSupply = MathF.Clamp(oldDemand / oldSupply, 0.5f, 1.5f); } // When EconomyTweak mod is initialized, oldSupply is set to 0.5-1.5.
			// float num = oldSupply * 0.85f + EconomyTweak_h_LongtermPriceMultiplier * 0.15f; Works fine too, but I changed it to (maybe?) more appropriate equation. It will change slower on extremely high values (it is long-term, after all), and daily change will not be capped at low values.
			float num = oldSupply * MathF.Pow((EconomyTweak_h_LongtermPriceMultiplier / oldSupply), 0.2f);
			// EconomyTweak_h_LongtermPriceMultiplier value is limited to 0.4 - 10.
			num = MathF.Clamp(num, 0.4f, 10f);
			__result = new ValueTuple<float, float>(num, item);
			if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 0)
			{
				using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt")) { Economy_log_writer_WriteText.WriteLine("EconomyTweak_h_GetSupplyDemandForCategory"); }
				if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugDisplay) { InformationManager.DisplayMessage(new InformationMessage("EconomyTweak_h_GetSupplyDemandForCategory")); }
				if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 1)
				{
					using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt"))
					{
						Economy_log_writer_WriteText.WriteLine("town = " + town.ToString() +
							", category = " + category.ToString() +
							", dailySupply = " + dailySupply.ToString() +
							", dailyDemand = " + dailyDemand.ToString() +
							", oldSupply = " + oldSupply.ToString() +
							", oldDemand = " + oldDemand.ToString() +
							", EconomyTweak_h_LongtermPriceMultiplier = " + EconomyTweak_h_LongtermPriceMultiplier.ToString() +
							", newDemand(item) = " + item.ToString() +
							", newSupply(num) = " + num.ToString() +
							", EconomyTweak_h_GetSupplyDemandForCategory(result) = " + __result.ToString());
					}
				}
			}
			return false; // Do not call the original method.
		}
	}

	[HarmonyPatch(typeof(DefaultTradeItemPriceFactorModel), "GetBasePriceFactor")]
	internal class EconomyTweak_h_GetBasePriceFactor_patch
	{
		// Method to calculate BasePriceFactor, which will determine prices together with default item values and trade penalty.
		// With EconomyTweak mod instead of original supply in supply value is stored EconomyTweak_h_LongtermPriceMultiplier.
		[HarmonyPrefix]
		public static bool EconomyTweak_h_GetBasePriceFactor(ItemCategory itemCategory, float inStoreValue, float supply, float demand, bool isSelling, int transferValue, ref float __result)
		{
			if (isSelling)
			{
				inStoreValue += (float)transferValue;
			}
			float EconomyTweak_h_OptimalStockValue = EconomyTweak_h_globalConstants.EconomyTweak_h_OptimalStockPeriodPrice * demand;
			// Short - term price multiplier is calculated (num).
			float num = MathF.Pow( (EconomyTweak_h_OptimalStockValue + demand + 1) / (inStoreValue + demand + 1), EconomyTweak_h_globalConstants.EconomyTweak_h_ShorttermPriceMultiplierExpValue);
			// Short-term price multiplier is multiplied by Long-term price multiplier.
            if (supply <= 10) // Check whether mod is already running for at least one day. Before one day change was required to get prices to "normal", now prices will be relatively reasonable even without that.
            {
			    num = num * supply;
            }
			// There could be problems when item category is Unassigned, as it has no demand, so caps are put for that category.
			if (itemCategory == DefaultItemCategories.Unassigned)
			{
				num = MathF.Clamp(num, 0.2f, 10f);
			}
			// In other cases caps should not be needed, but I will put them just in case.
			num = MathF.Clamp(num, 0.01f, 100f);
			__result = num;
			if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 2)
			{
				using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt")) { Economy_log_writer_WriteText.WriteLine("EconomyTweak_h_GetBasePriceFactor"); }
				if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugDisplay) { InformationManager.DisplayMessage(new InformationMessage("EconomyTweak_h_GetBasePriceFactor")); }
				if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 3)
				{
					using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt"))
					{
						Economy_log_writer_WriteText.WriteLine("itemCategory = " + itemCategory.ToString() +
							", inStoreValue = " + inStoreValue.ToString() +
							", supply = " + supply.ToString() +
							", demand = " + demand.ToString() +
							", isSelling = " + isSelling.ToString() +
							", transferValue = " + transferValue.ToString() +
							", EconomyTweak_h_OptimalStockValue = " + EconomyTweak_h_OptimalStockValue.ToString() +
							", EconomyTweak_h_GetBasePriceFactor(result) = " + __result.ToString());
					}
				}
			}
			return false; // Do not call the original method.
		}
	}

	[HarmonyPatch(typeof(ItemConsumptionBehavior), "MakeConsumption")]
	internal class EconomyTweak_h_MakeConsumption_patch
	{
		// Method to determine which items in town.Owner.ItemRoster will be consumed when the day changes. It depends on demand value for ItemCategory.
		// Originally items are consumed depending on their price, with EconomyTweak mod the demand is converted to item count using default value (with which it is determined in the first place), not the current price.
		// Originally items consumed are converted to town.Gold using the current price. With EconomyTweak mod it is also added to prosperity using convertion value EconomyTweak_h_ValueOfProsperity.
		// Originally demand not fulfilled is ignored. With EconomyTweak mod it has negative impact on prosperity.
		// With EconomyTweak mod consumption is intensified when there is too many items of category (more than needed for EconomyTweak_h_OptimalStockPeriod).
		[HarmonyPrefix]
		public static bool EconomyTweak_h_MakeConsumption(Town town, Dictionary<ItemCategory, float> categoryDemand, Dictionary<ItemCategory, int> saleLog)
		{

			if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 0)
			{
				using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt")) { Economy_log_writer_WriteText.WriteLine("EconomyTweak_h_MakeConsumption"); }
				if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugDisplay) { InformationManager.DisplayMessage(new InformationMessage("EconomyTweak_h_MakeConsumption")); }
			}
			EconomyTweak_h_Dictionaries.EconomyTweak_h_TownDemandFulfilledDictionary[town] = 0;
			saleLog.Clear();
			TownMarketData marketData = town.MarketData;
			ItemRoster itemRoster = town.Owner.ItemRoster;
			// Order of consumption is shuffled to prevent only the first items in category to be consumed.
			var numberList = Enumerable.Range(0, itemRoster.Count - 1).ToList();
			numberList.Shuffle();
			// For each item in town.Owner.ItemRoster category demand is fullfilled and is substracted from categoryDemand[itemCategory]. After that categoryDemand[itemCategory] is checked for unfulfilled demands.
			foreach (int i in numberList)
			{
				ItemObject itemAtIndex = itemRoster.GetItemAtIndex(i);
				int elementNumber = itemRoster.GetElementNumber(i);
				ItemCategory itemCategory = itemAtIndex.GetItemCategory();
				float demand = categoryDemand[itemCategory];
				// TownCategoryWorkshopPriceFactor and TownCategoryProsperityPriceFactor are removed from demand, as they are added in EconomyTweak_h_GetDailyDemandForCategory() only to influence prices.
				if (EconomyTweak_h_Dictionaries.EconomyTweak_h_TownCategoryWorkshopPriceFactorDictionary.ContainsKey((town, itemCategory)))
				{
					demand = demand / EconomyTweak_h_Dictionaries.EconomyTweak_h_TownCategoryWorkshopPriceFactorDictionary[(town, itemCategory)];
				}
				if (EconomyTweak_h_Dictionaries.EconomyTweak_h_TownCategoryProsperityPriceFactorDictionary.ContainsKey((town, itemCategory)))
				{
					demand = demand / EconomyTweak_h_Dictionaries.EconomyTweak_h_TownCategoryProsperityPriceFactorDictionary[(town, itemCategory)];
				}
				// When elementNumber (in which stock amount in town is stored) is greater than EconomyTweak_h_OptimalStockAmount, consumption is increased.
				float EconomyTweak_h_OptimalStockAmount = EconomyTweak_h_globalConstants.EconomyTweak_h_OptimalStockPeriodOverconsumption * demand / itemAtIndex.Value;
				float num = demand;
				if (elementNumber > EconomyTweak_h_OptimalStockAmount)
				{
					num = MBRandom.RoundRandomized(num * elementNumber / EconomyTweak_h_OptimalStockAmount);
				}
				if (num > 0.01f)
				{
					int price = marketData.GetPrice(itemAtIndex, null, false, null);
					// num2 is demanded quantity of item, num3 is part of that demand which is available in stocks to consume.
					int num2 = MBRandom.RoundRandomized(num / itemAtIndex.Value);
					int num3 = num2;
					if (num2 > elementNumber)
					{
						num3 = elementNumber;
					}
					if (num3 > elementNumber)
					{
						num3 = elementNumber;
					}
					// Items are consumed and their price is added to town.Gold by town.ChangeGold().
					itemRoster.AddToCountsAtIndex(i, -num3, false);
					town.ChangeGold(num3 * price);
					int num4 = 0;
					saleLog.TryGetValue(itemCategory, out num4);
					saleLog[itemCategory] = num4 + num3;
					// Consumed items are always added to town.Gold, but only itemCategory.IsTradeGood is added to town.Prosperity.
					if (itemCategory.IsTradeGood)
					{
						EconomyTweak_h_Dictionaries.EconomyTweak_h_TownDemandFulfilledAdd(town, num3 * itemAtIndex.Value / EconomyTweak_h_globalConstants.EconomyTweak_h_ValueOfProsperity);
					}
					// categoryDemand[itemCategory] after consumption is being set.
					categoryDemand[itemCategory] = (num2 - num3) * itemAtIndex.Value;
					if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 1)
					{
						using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt"))
						{
							Economy_log_writer_WriteText.WriteLine("EconomyTweak_h_MakeConsumption itemRoster.Count, i = " + i + ": " +
								", town = " + town.ToString() +
								", itemAtIndex.Name = " + itemAtIndex.Name.ToString() +
								", itemAtIndex.Value = " + itemAtIndex.Value.ToString() +
								", elementNumber = " + elementNumber.ToString() +
								", itemCategory = " + itemCategory.ToString() +
								", DemandValue(num) = " + num.ToString() +
								", DemandCount(num2) = " + num2.ToString() +
								", DemandCountAvail(num3) = " + num3.ToString() +
								", SoldBefore(num4) = " + num4.ToString() +
								", categoryDemand[itemCategory] = " + categoryDemand[itemCategory].ToString());
						}
					}
				}
			}
			// categoryDemand[itemCategory] is checked for unfulfilled demands.
			foreach (ItemObject itemObject in ItemObject.All)
			{
				ItemCategory iCategory = itemObject.GetItemCategory();
				if (categoryDemand[iCategory] > 0)
				{
					if (iCategory.IsTradeGood)
					{
						EconomyTweak_h_Dictionaries.EconomyTweak_h_TownDemandFulfilledAdd(town, -categoryDemand[iCategory] / EconomyTweak_h_globalConstants.EconomyTweak_h_ValueOfProsperity);
						categoryDemand[iCategory] = 0;
					}
				}
			}
			itemRoster.RemoveZeroCounts();
			List<Town.SellLog> list = new List<Town.SellLog>();
			foreach (KeyValuePair<ItemCategory, int> keyValuePair in saleLog)
			{
				if (keyValuePair.Value > 0)
				{
					list.Add(new Town.SellLog(keyValuePair.Key, keyValuePair.Value));
				}
			}
			// Instead of "town.SetSoldItems(list);", which can not be done from outside, Reflection was used.
			Town.SellLog[] _soldItems = new Town.SellLog[0];
			_soldItems = list.ToArray<Town.SellLog>();
			FieldInfo SellLogField = typeof(Town).GetField("_soldItems", BindingFlags.NonPublic | BindingFlags.Instance);
			SellLogField.SetValue(town, _soldItems);
			if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 1)
			{
				foreach (Town.SellLog sellLog in town.SoldItems)
				{
					using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt"))
					{
						Economy_log_writer_WriteText.WriteLine("EconomyTweak_h_MakeConsumption town.SoldItems: " +
							"town = " + town.ToString() +
							", sellLog.Category = " + sellLog.Category.ToString() +
							", sellLog.Number = " + sellLog.Number.ToString());
					}
				}
			}
			return false; // Do not call the original method.
		}
	}

	[HarmonyPatch(typeof(DefaultSettlementProsperityModel), "CalculateProsperityChange")]
	internal class EconomyTweak_h_CalculateProsperityChange_patch
	{
		// Method to calculate daily prosperity change and update corresponding explainedNumbers in tooltips. Originally there are many processes taken into account in CalculateProsperityChangeInternal().
		// EconomyTweak mod do not influence CalculateProsperityChangeInternal() (and it is internal too), it continues after that in CalculateProsperityChange().
		// EconomyTweak mod adds daily gold-prosperity conversion using EconomyTweak_h_ValueOfProsperity, with balance as follows: town.Prosperity * ValueOfProsperity = town.Gold.
		// EconomyTweak mod adds daily prosperity change due to demands fulfilled (or not).
		// EconomyTweak mod adds daily prosperity change due to food shortages. It pretty much prevents garrison starvation in normal conditions. After prosperity increases to the limit where it could no longer be sustained, prosperity starts to decrease before starvation (which is inevitable in the current game version without mods). Usually final balance is achieved at around 10-20% of food storage. Can have big impact on settlements prosperity when met with malice intent.
		[HarmonyPostfix]
		public static void EconomyTweak_h_CalculateProsperityChange(Town fortification, StatExplainer explanation, ref float __result)
		{
			int EconomyTweak_h_TownGold = fortification.Gold;
			float EconomyTweak_h_TownProsperity = (float)fortification.Prosperity;
			float EconomyTweak_h_FoodChange = fortification.FoodChange;
			float EconomyTweak_h_FoodStocks = fortification.FoodStocks;
			float EconomyTweak_h_TownDemandFulfilledValue = 0;
			float EconomyTweak_h_TargetGold = 0;
			int EconomyTweak_h_TargetGoldChange = 0;
			float EconomyTweak_h_FoodShortageProsperity = 0;
			ExplainedNumber explainedNumber_TownDemandFulfilled = new ExplainedNumber(0f, explanation, null);
			ExplainedNumber explainedNumber_TargetGoldChange = new ExplainedNumber(0f, explanation, null);
			ExplainedNumber explainedNumber_FoodShortageProsperity = new ExplainedNumber(0f, explanation, null);
			// Gold-prosperity convertion and demands fulfillment is not applicable to castles.
			if (!fortification.IsCastle)
			{
				// Depending on demands fulfilled (or not) prosperity changes, tooltip "Demands Fulfilled" appears and will be taken into account when day changes.
				if (EconomyTweak_h_Dictionaries.EconomyTweak_h_TownDemandFulfilledDictionary.ContainsKey(fortification))
				{
					TextObject EconomyTweak_h_TownDemandFulfilledText = new TextObject("Demands Fulfilled");
					if (EconomyTweak_h_Dictionaries.EconomyTweak_h_TownDemandFulfilledDictionary[fortification] < 0)
                    {
						EconomyTweak_h_TownDemandFulfilledValue = MathF.Pow(-EconomyTweak_h_Dictionaries.EconomyTweak_h_TownDemandFulfilledDictionary[fortification], EconomyTweak_h_globalConstants.EconomyTweak_h_TownDemandFulfilledProsperityExpValue);
                    }
                    else
					{
						EconomyTweak_h_TownDemandFulfilledValue = MathF.Pow(EconomyTweak_h_Dictionaries.EconomyTweak_h_TownDemandFulfilledDictionary[fortification], EconomyTweak_h_globalConstants.EconomyTweak_h_TownDemandFulfilledProsperityExpValue);
					}
					// Change is limited to 5% of the town prosperity.
					EconomyTweak_h_TownDemandFulfilledValue = MathF.Clamp(EconomyTweak_h_TownDemandFulfilledValue, -EconomyTweak_h_TownProsperity * 0.05f, EconomyTweak_h_TownProsperity * 0.05f);
					// Add explainedNumber_TownDemandFulfilled to tooltips and prosperity change result.
					explainedNumber_TownDemandFulfilled.Add(EconomyTweak_h_TownDemandFulfilledValue, EconomyTweak_h_TownDemandFulfilledText);
					__result += explainedNumber_TownDemandFulfilled.ResultNumber;
				}
				// Gold is converted to prosperity and prosperity to gold to achieve balance determined by EconomyTweak_h_ValueOfProsperity: gold and prosperity values of town are approaching equality.
				// Gold equal to EconomyTweak_h_TownProsperity * EconomyTweak_h_ValueOfProsperity is calculated, but it does not change to that amount in one day. EconomyTweak_h_TargetGold is calculated to become only 5% closer to that.
				EconomyTweak_h_TargetGold = EconomyTweak_h_TownGold * 0.95f + EconomyTweak_h_TownProsperity * EconomyTweak_h_globalConstants.EconomyTweak_h_ValueOfProsperity * 0.05f;
				EconomyTweak_h_TargetGoldChange = MBRandom.RoundRandomized(EconomyTweak_h_TargetGold - EconomyTweak_h_TownGold);
				// To account for gold change in GetTownGoldChange().
				EconomyTweak_h_TargetGold = (EconomyTweak_h_TownGold - EconomyTweak_h_TargetGoldChange) * 0.95f + EconomyTweak_h_TownProsperity * EconomyTweak_h_globalConstants.EconomyTweak_h_ValueOfProsperity * 0.05f;
				EconomyTweak_h_TargetGoldChange = MBRandom.RoundRandomized(EconomyTweak_h_TargetGold - EconomyTweak_h_TownGold);
				string EconomyTweak_h_TargetGoldChangeString = null;
				if (EconomyTweak_h_TargetGoldChange < 0)
				{
					EconomyTweak_h_TargetGoldChangeString = "Currency Investment";
				}
				else
				{
					EconomyTweak_h_TargetGoldChangeString = "Currency Withdrawal";
				}
				TextObject EconomyTweak_h_TargetGoldChangeText = new TextObject(EconomyTweak_h_TargetGoldChangeString);
				// Add explainedNumber_TargetGoldChange to tooltips and prosperity change result.
				explainedNumber_TargetGoldChange.Add(-EconomyTweak_h_TargetGoldChange / EconomyTweak_h_globalConstants.EconomyTweak_h_ValueOfProsperity, EconomyTweak_h_TargetGoldChangeText);
				__result += explainedNumber_TargetGoldChange.ResultNumber;
			}
			// Food shortages should affect prosperity changes of castles too.
			// Prosperity change due to food shortage becomes more significant when food stocks are less than optimal stock amount.
			// Towns will aspire to have stocks more than Prosperity / 50 + FoodChange * OptimalStockPeriodFood.
			float EconomyTweak_h_FoodDeficiency = EconomyTweak_h_FoodStocks + EconomyTweak_h_FoodChange * EconomyTweak_h_globalConstants.EconomyTweak_h_OptimalStockPeriodFood - EconomyTweak_h_TownProsperity / 50;
			if (EconomyTweak_h_FoodChange < 0 || EconomyTweak_h_FoodDeficiency < 0)
			{
				TextObject EconomyTweak_h_FoodShortageText = new TextObject("Food Supply Shortage");
				// Substracts from prosperity, but no more than 5% of the current prosperity.
				EconomyTweak_h_FoodShortageProsperity = MathF.Clamp(EconomyTweak_h_FoodChange - MathF.Pow(-Math.Min(0f, EconomyTweak_h_FoodDeficiency), EconomyTweak_h_globalConstants.EconomyTweak_h_FoodShortageProsperityExpValue), -EconomyTweak_h_TownProsperity * 0.05f, 0f);
				// Add explainedNumber_FoodShortageProsperity to tooltips and prosperity change result.
				explainedNumber_FoodShortageProsperity.Add(EconomyTweak_h_FoodShortageProsperity, EconomyTweak_h_FoodShortageText);
				__result += explainedNumber_FoodShortageProsperity.ResultNumber;
			}
			if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 0)
			{
				using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt")) { Economy_log_writer_WriteText.WriteLine("EconomyTweak_h_CalculateProsperityChange"); }
				if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugDisplay) { InformationManager.DisplayMessage(new InformationMessage("EconomyTweak_h_CalculateProsperityChange")); }
				if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 1)
				{
					using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt"))
					{
						Economy_log_writer_WriteText.WriteLine("fortification = " + fortification.ToString() +
							", EconomyTweak_h_TownProsperity = " + EconomyTweak_h_TownProsperity.ToString() +
							", EconomyTweak_h_TownGold = " + EconomyTweak_h_TownGold.ToString() +
							", EconomyTweak_h_TownDemandFulfilledValue = " + EconomyTweak_h_TownDemandFulfilledValue.ToString() +
							", explainedNumber_TownDemandFulfilled = " + explainedNumber_TownDemandFulfilled.ResultNumber.ToString() +
							", EconomyTweak_h_TargetGold = " + EconomyTweak_h_TargetGold.ToString() +
							", EconomyTweak_h_TargetGoldChange = " + EconomyTweak_h_TargetGoldChange.ToString() +
							", explainedNumber_TargetGoldChange = " + explainedNumber_TargetGoldChange.ResultNumber.ToString() +
							", EconomyTweak_h_FoodChange = " + EconomyTweak_h_FoodChange.ToString() +
							", EconomyTweak_h_FoodStocks = " + EconomyTweak_h_FoodStocks.ToString() +
							", EconomyTweak_h_FoodShortageProsperity = " + EconomyTweak_h_FoodShortageProsperity.ToString() +
							", explainedNumber_FoodShortageProsperity = " + explainedNumber_FoodShortageProsperity.ResultNumber.ToString() +
							", EconomyTweak_h_CalculateProsperityChange(result) = " + __result.ToString());
					}
				}
			}
		}
	}

	[HarmonyPatch(typeof(DefaultSettlementEconomyModel), "GetTownGoldChange")]
	internal class EconomyTweak_h_GetTownGoldChange_patch
	{
		// Originally GetTownGoldChange() adds artificial sink for gold with "float num = town.Prosperity * 7f - (float)town.Gold;". That means that when settlement has (gold > prosperity * 7), gold just disappears, 20% of that surplus in one day "MathF.Round(0.2f * num);". Or the gold appears from nowhere, if you will take all the money by selling stuff, for example.
		// Obviously that is not compatible well with gold-prosperity conversion and self-balancing idea.
		// So EconomyTweak mod changes that balance to Prosperity * ValueOfProsperity = Gold.
		[HarmonyPrefix]
		public static bool EconomyTweak_h_GetTownGoldChange(Town town, ref int __result)
		{
			int EconomyTweak_h_TownGold = town.Gold;
			float EconomyTweak_h_TownProsperity = (float)town.Prosperity;
			float EconomyTweak_h_TargetGold = 0;
			int EconomyTweak_h_TargetGoldChange = 0;
			// Gold equal to EconomyTweak_h_TownProsperity * EconomyTweak_h_ValueOfProsperity is calculated, but it does not change to that amount in one day. EconomyTweak_h_TargetGold is calculated to become only 5% closer to that.
			EconomyTweak_h_TargetGold = EconomyTweak_h_TownGold * 0.95f + EconomyTweak_h_TownProsperity * EconomyTweak_h_globalConstants.EconomyTweak_h_ValueOfProsperity * 0.05f;
			EconomyTweak_h_TargetGoldChange = MBRandom.RoundRandomized(EconomyTweak_h_TargetGold - EconomyTweak_h_TownGold);
			__result = EconomyTweak_h_TargetGoldChange;
			if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 0)
			{
				using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt")) { Economy_log_writer_WriteText.WriteLine("EconomyTweak_h_GetTownGoldChange"); }
				if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugDisplay) { InformationManager.DisplayMessage(new InformationMessage("EconomyTweak_h_GetTownGoldChange")); }
				if (EconomyTweak_h_globalConstants.EconomyTweak_h_DebugLevel > 1)
				{
					using (StreamWriter Economy_log_writer_WriteText = File.AppendText(EconomyTweak_h_globalConstants.EconomyTweak_h_path + "EconomyTweak_h_log.txt"))
					{
						Economy_log_writer_WriteText.WriteLine("town = " + town.ToString() +
							", EconomyTweak_h_TownGold = " + EconomyTweak_h_TownGold.ToString() +
							", EconomyTweak_h_TownProsperity = " + EconomyTweak_h_TownProsperity.ToString() +
							", EconomyTweak_h_GetTownGoldChange(result) = " + __result.ToString());
					}
				}
			}
			return false; // Do not call the original method.
		}
	}

	[HarmonyPatch(typeof(DefaultVillageProductionCalculatorModel), "CalculateDailyProductionAmount")]
	internal class EconomyTweak_h_CalculateDailyProductionAmount_patch
	{
		[HarmonyPostfix]
		public static void EconomyTweak_h_CalculateDailyProductionAmount(Village village, ItemObject item, ref float __result)
		{
			__result = __result * EconomyTweak_h_globalConstants.EconomyTweak_h_ProductionMultiplier; // Production amount of all villages is multiplied by EconomyTweak_h_ProductionMultiplier.
		}
	}

	[HarmonyPatch(typeof(CaravansCampaignBehavior), "SpawnCaravans")]
	internal class EconomyTweak_h_SpawnCaravans_patch
	{
		[HarmonyPrefix]
		public static bool EconomyTweak_h_SpawnCaravans(bool initialSpawn = false)
		{
			foreach (Hero hero in Hero.All)
			{
//				Removed "DaysLeftToRespawn" method as it doesn't exist in 1.4.3. Replaced with "IsDisabled" in case it works for the same effect.
//				bool ShouldHaveCaravan = hero.PartyBelongedTo == null && hero.IsMerchant && (hero.IsFugitive || hero.IsReleased || hero.IsNotSpawned || hero.IsActive) && hero.DaysLeftToRespawn == 0 && !hero.IsTemplate && !hero.IsOccupiedByAnEvent();
				bool ShouldHaveCaravan = hero.PartyBelongedTo == null && hero.IsMerchant && (hero.IsFugitive || hero.IsReleased || hero.IsNotSpawned || hero.IsActive) && hero.IsDisabled && !hero.IsTemplate && !hero.IsOccupiedByAnEvent();
				if (hero != Hero.MainHero && ShouldHaveCaravan && hero.OwnedCaravans.Count<MobileParty>() <= 1) // The only change of that method: "hero.OwnedCaravans.Count<MobileParty>() <= 0" condition is replaced by "hero.OwnedCaravans.Count<MobileParty>() <= 1". It should let heroes spawn up to two caravans instead of only one.
				{
					Settlement spawnSettlement = hero.HomeSettlement.IsFortification ? hero.HomeSettlement : hero.HomeSettlement.GetComponent<Village>().TradeBound;
					MobileParty.InitializeCaravanForHero(hero, spawnSettlement, initialSpawn, null, null, 0);
					if (!initialSpawn && hero.Power >= 50)
					{
						hero.AddPower(-50f);
					}
				}
			}
			return false; // Do not call the original method.
		}
	}
}
