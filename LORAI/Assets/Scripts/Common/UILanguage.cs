public class UILanguage
{
	public UITitle uiTitle;
	public UISetup uiSetup;
	public UIMainApp uiMainApp;
}

public class UITitle
{
	/*
		"menuHeading": "command",
		"newGameBtn": "new game",
		"continueBtn": "continue",
		"campaignsBtn": "campaigns",
		"optionsBtn": "options",
	*/
	public string menuHeading, newGameBtn, continueBtn, campaignsBtn, optionsBtn, supportUC;
}

public class UISetup
{
	/*
		"settingsHeading": "mission settings",
		"chooseMission": "choose mission",
		"viewCardBtn": "view",
		"missionInfoBtn": "mission info",
		"threatLevelHeading": "threat level",
		"addtlThreatHeading": "additional threat",
		"deploymentHeading": "optional deployment",
		"yes": "yes",
		"no": "no",
		"back": "back",
		"difficulty": "difficulty",
		"easy": "easy",
		"normal": "normal",
		"hard": "hard",
		"imperials": "imperials",
		"mercenaries": "merecenaries",
		"adaptive": "adaptive difficulty",
		"groupsHeading": "enemy groups",
		"choose": "choose",
		"zoom": "zoom",
		"initialHeading": "initial",
		"reservedHeading": "reserved",
		"villainsHeading": "earned villains",
		"ignoredHeading": "ignored",
		"addHero": "add hero",
		"addAlly": "add ally",
		"threatCostHeading": "threat cost?",
		"cancel": "cancel",
		"continueBtn": "continue",
		"saved": "saved",
		"loaded": "loaded",
		"selected": "selected",
		"enemyChooser": "deployment groups",
		"missionChooser": "select a mission",
		"heroAllyChooser": "choose one",
		"adaptiveInfoUC": "Adaptive Difficulty: The more Imperial groups you manage to defeat, the harder the Empire tries to stop you, but the better your rewards."
	 */
	public string settingsHeading, chooseMission, viewCardBtn, missionInfoBtn, threatLevelHeading, addtlThreatHeading, deploymentHeading, yes, no, back, difficulty, easy, normal, hard, imperials, mercenaries, adaptive, groupsHeading, choose, zoom, initialHeading, reservedHeading, villainsHeading, ignoredHeading, addHero, addAlly, threatCostHeading, cancel, continueBtn, saved, loaded, selected, enemyChooser, missionChooser, heroAllyChooser, adaptiveInfoUC;
}

public class UIMainApp
{
	public string eliteUpgrade, eliteDowngrade;
}
