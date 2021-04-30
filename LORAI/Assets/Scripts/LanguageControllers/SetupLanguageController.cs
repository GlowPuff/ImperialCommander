using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupLanguageController : MonoBehaviour
{
	public Text settingsHeader, chooseMissionBtn, viewCardBtn, missionInfoBtn, threatLevel, addtlThreat, deploymentHeader, opdepBtn, difficultyBtn, imperialsHeader, mercenariesHeader, adaptiveHeader, groupsHeading, initialBtn, reservedBtn, villainsBtn, ignoredBtn, initialHeading, reservedHeading, villainsHeading, ignoredHeading, addHeroBtn, addAllyBtn, threatCostHeading, threatCostBtn, cancelBtn, continueBtn, prefsStatus, enemyChooserHeading, missionChooserHeading, heroAllyChooserHeading, enemyBackBtn, missionBackBtn, heroAllyBackBtn, zoomBackBtn, enemyZoomBtn;
	public TextMeshProUGUI adaptiveInfo;

	/// <summary>
	/// Sets the UI with the current language
	/// </summary>
	public void SetTranslatedUI()
	{
		UISetup uis = DataStore.uiLanguage.uiSetup;

		settingsHeader.text = uis.settingsHeading;
		chooseMissionBtn.text = uis.chooseMission;
		viewCardBtn.text = uis.viewCardBtn;
		missionInfoBtn.text = uis.missionInfoBtn;
		threatLevel.text = uis.threatLevelHeading;
		addtlThreat.text = uis.addtlThreatHeading;
		deploymentHeader.text = uis.deploymentHeading;
		opdepBtn.text = uis.no;
		difficultyBtn.text = uis.difficulty;
		imperialsHeader.text = uis.imperials;
		mercenariesHeader.text = uis.mercenaries;
		adaptiveHeader.text = uis.adaptive;
		groupsHeading.text = uis.groupsHeading;
		initialBtn.text = uis.choose;
		reservedBtn.text = uis.choose;
		villainsBtn.text = uis.choose;
		ignoredBtn.text = uis.choose;
		initialHeading.text = uis.initialHeading;
		reservedHeading.text = uis.reservedHeading;
		villainsHeading.text = uis.villainsHeading;
		ignoredHeading.text = uis.ignoredHeading;
		addHeroBtn.text = uis.addHero;
		addAllyBtn.text = uis.addAlly;
		threatCostHeading.text = uis.threatCostHeading;
		threatCostBtn.text = uis.no;
		cancelBtn.text = uis.cancel;
		continueBtn.text = uis.continueBtn;
		adaptiveInfo.text = uis.adaptiveInfoUC;
		enemyChooserHeading.text = uis.enemyChooser;
		missionChooserHeading.text = uis.missionChooser;
		heroAllyChooserHeading.text = uis.heroAllyChooser;
		enemyBackBtn.text = uis.back;
		missionBackBtn.text = uis.back;
		heroAllyBackBtn.text = uis.back;
		zoomBackBtn.text = uis.back;
		enemyZoomBtn.text = uis.zoom;
	}
}
