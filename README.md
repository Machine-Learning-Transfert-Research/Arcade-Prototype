# Arcade Prototype
Arcade prototype for the research project: [**Evaluating transfer learning for reinforcement learning agents across same-genre video games**](https://github.com/Machine-Learning-Transfert-Research) <br>
Research project carried out in partnership between [PulluP Entertainment](https://pullupent.com/en) and [ISART Digital Paris](https://www.isart.fr/)

<p align="center">
    <img src="./Readme/pullup_logo.jpg" width="200" height="200" alt="pullup logo"/>
    <img src="./Readme/isart_logo.jpg"  width="200" height="200" alt="isart logo"/>
</p>

[Realistic Prototype Github](https://github.com/Machine-Learning-Transfert-Research/Realistic-Prototype)

## Table of Content
- [Setup](#setup)
- [Technology](#technology)
- [Credit](#credit)

## Setup
### Start a training session
In [**Anaconda Prompt**](https://www.anaconda.com/download)
1. Go in the project folder, then, in the ```Assets\ML-Agents``` folder
2. Activate ML Agents
```
conda activate mlagents
```
3. Start training *(replace XXX by the training number, ex: 005)*
```
mlagents-learn Config\trainer_config.yaml --run-id=trainingXXX
```

In **Unity** 
1. Open the scenes ```Assets\scenes\Training```
2. Click on the **play** button

### Resume a existing training section

In [**Anaconda Prompt**](https://www.anaconda.com/download)
1. Go in the project folder, then, in the ```Assets\ML-Agents``` folder
2. Activate ML Agents
```
conda activate mlagents
```
3. Resume training *(replace XXX by the training number, ex: 005)*
```
mlagents-learn Config\trainer_config.yaml --run-id=trainingXXX --resume
```

In **Unity** 
1. Open the scenes ```Assets\scenes\Training```
2. Click on the **play** button

### Test trained agent
In **Unity** 
1. Open the scenes ```Assets\scenes\Testing```
2. In the *Hierarchy* ```Car -> MovementRigibody``` GameObject
3. In the *Inspector* ```Behavior Parameters``` Script
4. Change the parameters ```Model``` with the neural network you want to test. Make sure that ```Behavior Type``` is set to *Inference Only*
5. Click on the **play** button

### Evaluate trained agent
In **Unity** 
1. Open the scenes ```Assets\scenes\Testing```
2. In the *Hierarchy* ```EnvTraining``` GameObject
3. In the *Inspector* ```Evaluation Tests``` Script
4. Set the parameters ```Model Tested``` with the neural network you want to evaluate
5. Click on the **play** button

## Technology
- Unity 6 *v6000.3.9f1*
- [ML Agents Plugin](https://docs.unity3d.com/Packages/com.unity.ml-agents@4.0/manual/index.html) *(version 4.0.2)*

## Credits
- [Bryan BACHELET](https://www.linkedin.com/in/bryan-bachelet/)
- [Vincent DEVINE](https://www.linkedin.com/in/vincent-devine/)
- [Matéo ERBISTI](https://www.linkedin.com/in/mat%C3%A9o-erbisti/)
- [Omaya LISE](https://www.linkedin.com/in/omaya-lise/)
- [Aurelien CHAMBON](https://www.linkedin.com/in/aurelien-chambon/)
- [Aurélien LHERBIER](https://www.linkedin.com/in/aur%C3%A9lien-lherbier-a344993b/)

### Assets
- [Dreamteck Spline by Dreamteck](https://assetstore.unity.com/packages/tools/utilities/dreamteck-splines-61926)
- [Rally cars by Ash Dev](https://assetstore.unity.com/packages/3d/vehicles/rally-cars-215152)
- [Racing Kit by Kenny](https://kenney.nl/assets/racing-kit)