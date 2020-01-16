﻿using System;
using UnityEngine;
using KSP.UI.Screens;

namespace LRTR
{
    public class TopWindow : UIBase
    {
        // GUI
        static Rect windowPos = new Rect(500, 240, 0, 0);
        private MaintenanceGUI maintUI = new MaintenanceGUI();
        private Crew.FSGUI fsUI = new LRTR.Crew.FSGUI();
        private static tabs currentTab;

        public TopWindow()
        {
            // Reset the tab on scene changes
            currentTab = default(tabs);
        }

        public void OnGUI()
        {
            windowPos = GUILayout.Window("LRTRTop".GetHashCode(), windowPos, DrawWindow, "LRTR");
        }

        public static void SwitchTabTo(tabs newTab)
        {
            currentTab = newTab;
        }

        private void tabSelector()
        {
            GUILayout.BeginHorizontal();
            try {
                if (showTab(tabs.Maintenance) && toggleButton("Maintenance", currentTab == tabs.Maintenance))
                    currentTab = tabs.Maintenance;
                if (showTab(tabs.Training) && toggleButton("Astronauts", currentTab == tabs.Training))
                    currentTab = tabs.Training;
                if (showTab(tabs.Courses) && toggleButton("Courses", currentTab == tabs.Courses))
                    currentTab = tabs.Courses;
            } finally {
                GUILayout.EndHorizontal();
            }
        }

        public void DrawWindow(int windowID)
        {
            try {
                GUILayout.BeginVertical();
                try {
                    /* If totalUpkeep is zero, we probably haven't calculated the upkeeps yet, so recalculate now */
                    if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER && MaintenanceHandler.Instance.totalUpkeep == 0d)
                        MaintenanceHandler.Instance.UpdateUpkeep();

                    tabSelector();
                    if (showTab(currentTab)) {
                        switch (currentTab) {
                        case tabs.Maintenance:
                            maintUI.summaryTab();
                            break;
                        case tabs.Facilities:
                            maintUI.facilitiesTab();
                            break;
                        case tabs.Integration:
                            maintUI.integrationTab();
                            break;
                        case tabs.Astronauts:
                            maintUI.astronautsTab();
                            break;
                        case tabs.Training:
                            currentTab = fsUI.summaryTab();
                            break;
                        case tabs.Courses:
                            currentTab = fsUI.coursesTab();
                            break;
                        case tabs.NewCourse:
                            currentTab = fsUI.newCourseTab();
                            break;
                        case tabs.Naut:
                            fsUI.nautTab();
                            break;
                        default: // can't happen
                            break;
                        }
                    }
                } finally {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();
                }
            } finally {
                GUI.DragWindow();
            }
        }
    }
}

