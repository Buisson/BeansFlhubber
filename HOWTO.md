# How To

## Besoins

Pour utiliser les différents services développés, vous devez au préalable installer plusieurs composants sur votre machine hôte (testé avec Windows 10 64bit).
 
- Installez SharpDevelop 3.2, ainsi que l'AddIn SharpWComp-3.2.
- Installez Node.js. Dans le dossier serveur/sources/nodejs, lancer la commande : "npm install".
- Logiciel Device Spy.

## Ordre de lancement

### Objet Connecté

Commencez par branchez l'objet connecté :
- Branchez le câble ethernet ;
- Branchez les deux prises d'alimentation : celle de la RaspberryPi et celle du hub.

Au démarrage, la RaspberryPi va lancer le script Python (grace au fichier /etc/rc.local) et ainsi devenir l'objet connecté FlHubber.

### Services

#### Serveur Node.js

Dans le dossier serveur/sources/nodejs, lancer le serveur Node.js avec la commande :
node ./server.js

#### Schéma de composition WComp

- Déplacez toutes les DLL du dossier wcom/bin dans votre répertoire WComp (généralement Documents/WComp.NET/Beans).
- Ouvrez SharpDevelop. 
- Créez un nouveau fichier "C# Container" dans la catégorie "WComp.NET".
- Dans le menu "WComp.NET", choisissez "Import (Replace)..." et sélectionnez le fichier "FlHubber.wcc" présent dans le dossier wcom/sources.
- Cliquez sur le bouton "Start", lié au Bean "FlHubberService", qui va lancer le Bean serveur.
- Lancez le logiciel "Device Spy", sélectionnez l'objet "PyUPnP_Flhubber", copiez l'adresse de l'objet.
- Dans le schéma WComp, sélectionner le Bean FlHubber (de type PyUPnP_Flhubber), et copier l'adresse dans l'Uri (dans les propriétés).


// TODO: Faire une doc pour la mise en place de votre projet (Step by step) 
