using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap.Unity;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Yuanju.Interfaces_and_classes.generator_components
{
    class SteamGenerator : MonoBehaviour
    {
        public GameObject steamGenerator;

        void Update()
        {

        }

        public SteamGenerator(GameObject steamGenerator)
        {
            this.steamGenerator = steamGenerator;
        }

        public List<Serializer.item> GetComponentsStatus() {

            List<Serializer.item> components = new List<Serializer.item>();
            var childrenList = new List<Transform>();
            steamGenerator.transform.GetAllChildren(childrenList);

            foreach(string compName in NPOIGetSequenceTable.OperableComponentNames)
            {
                var compGameObject = childrenList.Find(comp => comp.name == compName);

                if (compGameObject)
                {
                    var switchComponent = compGameObject.GetComponent<Switch>();
                    var buttonComponent = compGameObject.GetComponent<Button>();
                    var valveComponent = compGameObject.GetComponent<Valvola>();
                    var pressureComponent = compGameObject.GetComponent<LiquidCristalDisplay>();


                    var powerBloched = 0;
                    var status = 0;
                    if (switchComponent)
                    {
                        //powerBloched = switchComponent.IsPowerConnected;
                        status = switchComponent.Status;
                    }
                    else if (buttonComponent)
                    {
                        status = buttonComponent.Status;
                    }
                    else if(valveComponent)
                    {
                        status = valveComponent.Status;
                    }
                    else if(pressureComponent) {
                        status = pressureComponent.Status;
                    }


                    var comp = new Serializer.item(compName, status, powerBloched);
                    //var comp = new Serializer.item(compName, switchComponent.Status, powerBloched);
                    components.Add(comp);

                }
                else
                {
                    Debug.Log("comp da serializzare non trovata " + compName);
                }
            }

            return components;

        }

        public List<Serializer.item> SetComponentsStatus(List<Serializer.item> components) {

            var childrenList = new List<Transform>();
            steamGenerator.transform.GetAllChildren(childrenList);
            foreach(Serializer.item itemComp in components) {
                var compGameObject = childrenList.Find(comp => comp.name == itemComp.name);
                //Debug.Log("comp da serializzare " + itemComp.name);

                if(compGameObject) {
                    var switchComponent = compGameObject.GetComponent<Switch>();
                    var buttonComponent = compGameObject.GetComponent<Button>();
                    var gaugeComponent = compGameObject.GetComponent<PressureGauge>();
                    var waterLevelIndicatorComponent = compGameObject.GetComponent<WaterLevelIndicator>();
                    var waterTankComponent = compGameObject.GetComponent<WaterTank>();
                    var ledComponent = compGameObject.GetComponent<LED>();

                    if(switchComponent) {
                        if (itemComp.powerBlock == 0)
                        {
                            switchComponent.IsPowerConnected = false;
                        }
                        else
                        {
                            switchComponent.IsPowerConnected = true;
                        }

                        //Debug.Log(String.Format("switch {0} aggiornato stato {1}", itemComp.name, switchComponent.Status));
                    }
                    else if(gaugeComponent)
                    {
                        double temporaryStatus = itemComp.status;
                        gaugeComponent.Status = (int)Math.Round(temporaryStatus, 0);
                        //Debug.Log(String.Format("pressione {0} aggiornato stato {1}", itemComp.name, itemComp.status));
                    }
                    else if(waterLevelIndicatorComponent) {
                        double temporaryStatus = itemComp.status;
                        waterLevelIndicatorComponent.Status = (int)Math.Round(temporaryStatus, 0);
                        //Debug.Log(String.Format("water level {0} aggiornato stato {1}", temporaryStatus, waterLevelIndicatorComponent.Status));
                    }
                    else if(waterTankComponent) {
                        double temporaryStatus = itemComp.status;
                        waterTankComponent.Status = (int)Math.Round(temporaryStatus, 0);
                        //Debug.Log(String.Format("pressione {0} aggiornato stato {1}", itemComp.name, itemComp.status));
                    }
                    else if(ledComponent) {
                        if (itemComp.status==1)
                        {
                            ledComponent.Status = 1;
                        }
                        else
                        {
                            ledComponent.Status = 0;
                        }
                        
                        //Debug.Log(String.Format("pressione {0} aggiornato stato {1}", itemComp.name, itemComp.status));
                    } else
                    {
                        Debug.Log("classe della comp da deserializzare non trovata " + itemComp.name);
                    }


                } else {
                    Debug.Log("comp da deserializzare non trovata " + itemComp.name);
                }
            }

            return components;

        }

        public Serializer.list SerializeGeneratorStatus()
        {
            var componentsStatusList = GetComponentsStatus();
            var compList = new Serializer.list(componentsStatusList);
            return compList;
        }

        public string GetJson(Serializer.list compList)
        {
            string json = JsonUtility.ToJson(compList, true);
            Debug.Log(json);

            return json;
        }

        public List<Serializer.item> DeserializeGeneratorStatus(string json) {

            //Debug.Log(json);
            var compList = JsonUtility.FromJson<Serializer.list>("{\"componentList\":" + json + "}");
            return compList.componentList;
        }
    }
}
