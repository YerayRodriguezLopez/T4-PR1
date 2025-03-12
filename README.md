# T4. PR1. Pràctica 1 - Documentació

## 1. Diagrames  
Cada diagrama ha de ser explicat detalladament, indicant els seus components, relacions, restriccions i multiplicitats.

### 1.1. Diagrama de classes  
El projecte implementa diverses classes seguint els principis de **Programació Orientada a Objectes (OOP)**. Les classes principals són:

- `AbstractEnergySystem`: Classe base per als diferents sistemes d'energia renovable.
- `SolarEnergySystem`, `WindEnergySystem`, `HydroEnergySystem`: Subclasses que implementen càlculs específics d'energia.
- `Simulation`: Model per emmagatzemar informació de cada simulació.
- `SimulationService`: Servei per gestionar la persistència de dades en CSV, XML i JSON.

**Relacions i multiplicitats:**  
- `Simulation` conté una instància de `EnergyType` com a enumeració.
- `SimulationService` pot gestionar múltiples instàncies de `Simulation`.

### 1.2. Diagrama de casos d'ús  
Els principals casos d'ús de l'aplicació són:

1. **Crear una nova simulació**
   - Actor: Usuari
   - Procés: L'usuari introdueix dades i selecciona el tipus d'energia. El sistema valida i guarda la simulació.

2. **Consultar simulacions guardades**
   - Actor: Usuari
   - Procés: L'usuari accedeix a la pàgina de simulacions i visualitza un informe tabular.

3. **Exportar dades a CSV, XML i JSON**
   - Actor: Usuari
   - Procés: L'usuari selecciona un format i descarrega les dades.

## 2. GitHub Project 
- **Justificació de la configuració:**
  - S'ha seguit una estructura modular per facilitar mantenibilitat i escalabilitat.

- **URL del repositori:**
  - *(Afegir enllaç al repositori de GitHub)*


## 3. Solució del sistema  
- **Repositori i branca principal:** https//:github.com/YerayRodriguezLopez/T4-PR1
- **Document README:** Explicació del sistema i instruccions d'ús.
- **Guia d'estil:** Normes de nomenclatura i estructuració del codi.

## 4. Testing de la solució  
| Classe d'equivalència | Cas límit | Cas de prova | Resultat esperat |
|----------------------|----------|--------------|------------------|
| SolarEnergySystem   | Hores sol = 0 | Càlcul energia generada | 0 kWh |
| WindEnergySystem    | Velocitat vent = 1 | Càlcul energia generada | Valor correcte |
| HydroEnergySystem   | Cabal aigua = 10 | Càlcul energia generada | Valor correcte |

## 5. Bibliografia  
1. **Microsoft Docs** - Razor Pages: https://learn.microsoft.com/en-us/aspnet/core/razor-pages/
2. **SonarQube** - Qualitat del codi: https://www.sonarqube.org/
3. **XUnit** - Testing en C#: https://xunit.net/
