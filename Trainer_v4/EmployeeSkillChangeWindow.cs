﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils = Trainer_v4.Utilities;

namespace Trainer_v4
{
    public class EmployeeSkillChangeWindow : MonoBehaviour
    {
        private static string _title = "Employee Skill Change, by Trawis";

        public static GUIWindow Window;
        public static bool Shown;

        public static void Show()
        {
            if (Shown)
            {
                Window.Close();
                Shown = false;
                return;
            }

            CreateWindow();
            Shown = true;
        }

        private static void CreateWindow()
        {
            Window = WindowManager.SpawnWindow();
            Window.InitialTitle = Window.TitleText.text = Window.NonLocTitle = _title;
            Window.name = "EmployeeSkillChange";
            Window.MainPanel.name = "EmployeeSkillChangePanel";

            if (Window.name == "EmployeeSkillChange")
            {
                Window.GetComponentsInChildren<Button>()
                  .SingleOrDefault(x => x.name == "CloseButton")
                  .onClick.AddListener(() => Shown = false);
            }

            List<GameObject> roleToggles = new List<GameObject>();
            List<GameObject> specializationToggles = new List<GameObject>();

            Utils.AddLabel("Roles", new Rect(10, 5, 150, 32), Window);
            Utils.AddLabel("Specializations", new Rect(161, 5, 150, 32), Window);

            var rolesList = PropertyHelper.RolesList;
            foreach (var role in rolesList)
            {
                Utils.AddToggle(role.Key, PropertyHelper.GetProperty(rolesList, role.Key), 
                                     a => PropertyHelper.SetProperty(rolesList, role.Key, 
                                         !PropertyHelper.GetProperty(rolesList, role.Key)), 
                                     roleToggles);
            }

            Utils.AddButton("Set Skills", TrainerBehaviour.SetSkillPerEmployee, roleToggles);

            var specializationsList = PropertyHelper.SpecializationsList;
            foreach (var specialization in specializationsList)
            {
                Utils.AddToggle(specialization.Key,
                                PropertyHelper.GetProperty(specializationsList, specialization.Key),
                                a => PropertyHelper.SetProperty(specializationsList, specialization.Key, 
                                    !PropertyHelper.GetProperty(specializationsList, specialization.Key)),
                                specializationToggles);
            }

            Utils.CreateGameObjects(Constants.FIRST_COLUMN, 1, roleToggles.ToArray(), Window);
            Utils.CreateGameObjects(Constants.SECOND_COLUMN, 1, specializationToggles.ToArray(), Window);

            int[] columnsCount = new int[]
            {
                roleToggles.Count(),
                specializationToggles.Count()
            };

            Utils.SetWindowSize(columnsCount, Constants.X_EMPLOYEESKILLCHANGE_WINDOW, Constants.Y_EMPLOYEESKILLCHANGE_WINDOW_OFFSET, Window);
        }
    }
}