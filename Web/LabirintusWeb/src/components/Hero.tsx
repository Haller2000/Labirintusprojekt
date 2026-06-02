import type { Language } from '../App';

type HeroProps = {
  language: Language;
};

function Hero({ language }: HeroProps) {
  const text = {
    hu: {
      badge: 'Felhasználói dokumentáció',
      title: 'Labirintus játék és térképszerkesztő',
      description:
        'A projekt egy konzolos labirintus játékból és egy grafikus térképszerkesztőből áll. A játékban kincses termeket kell megtalálni, majd ki kell jutni a labirintusból. A szerkesztővel saját pályák készíthetők és ellenőrizhetők.',
      gameButton: 'Játék használata',
      editorButton: 'Szerkesztő használata',
      movement: 'irányítás',
      languageSelector: 'nyelvválasztás',
      saveLoad: 'mentés és betöltés',
    },
    en: {
      badge: 'User documentation',
      title: 'Labyrinth Game and Map Editor',
      description:
        'This project consists of a console-based labyrinth game and a graphical map editor. In the game, you must find every treasure room and then escape from the labyrinth. With the editor, you can create, edit and validate your own maps.',
      gameButton: 'How to play',
      editorButton: 'Use the editor',
      movement: 'movement',
      languageSelector: 'language selector',
      saveLoad: 'save and load',
    },
  }[language];

  return (
    <header id="top" className="hero">
      <div className="hero-content">
        <span className="badge">{text.badge}</span>
        <h1>{text.title}</h1>
        <p>{text.description}</p>

        <div className="hero-actions">
          <a className="button primary" href="#jatek">
            {text.gameButton}
          </a>
          <a className="button secondary" href="#szerkeszto">
            {text.editorButton}
          </a>
        </div>
      </div>

      <div className="hero-panel">
        <pre>{`╔════╗....╔══╗
║....╚════╝..║
║.██....╔════╝
╚════╦══╝....█
.....╚═══════╝`}</pre>

        <div className="stats">
          <div>
            <strong>WASD</strong>
            <span>{text.movement}</span>
          </div>
          <div>
            <strong>HU / EN</strong>
            <span>{text.languageSelector}</span>
          </div>
          <div>
            <strong>F5 / F9</strong>
            <span>{text.saveLoad}</span>
          </div>
        </div>
      </div>
    </header>
  );
}

export default Hero;