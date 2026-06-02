import type { Language } from '../App';
import ImagePlaceholder from './ImagePlaceholder';

type GameSectionProps = {
  language: Language;
};

function GameSection({ language }: GameSectionProps) {
  const text = {
    hu: {
      title: 'Játékprogram',
      intro:
        'A játék célja az összes kincses terem felfedezése, majd a kijáraton keresztüli menekülés.',
      imageTitle: 'Játék főképernyő',
      imageDescription: 'Ide kerül a játék közbeni képernyőkép.',
      controls: 'Irányítás',
      modes: 'Játékmódok',
      save: 'Mentés és betöltés',
      up: 'Felfelé mozgás',
      left: 'Balra mozgás',
      down: 'Lefelé mozgás',
      right: 'Jobbra mozgás',
      saveKey: 'Mentés',
      loadKey: 'Betöltés',
      exitKey: 'Kilépés',
      fullMap: 'Teljes térkép mód',
      fogMap: 'Fedett térkép mód',
      timeLimit: '120 másodperces időkorlát',
      fogTitle: 'Fedett térkép mód',
      fogDescription: 'A vaktérkép működésének bemutatása.',
      saveText:
        'A játék támogatja a mentést és a visszatöltést. A játékállapot egy .sav állományba kerül mentésre.',
    },
    en: {
      title: 'Game',
      intro:
        'The goal of the game is to discover every treasure room and then escape through an exit.',
      imageTitle: 'Game screen',
      imageDescription: 'Place a screenshot of the running game here.',
      controls: 'Controls',
      modes: 'Game modes',
      save: 'Saving and loading',
      up: 'Move up',
      left: 'Move left',
      down: 'Move down',
      right: 'Move right',
      saveKey: 'Save',
      loadKey: 'Load',
      exitKey: 'Exit',
      fullMap: 'Full map mode',
      fogMap: 'Fog of war mode',
      timeLimit: '120 second time limit',
      fogTitle: 'Fog of war mode',
      fogDescription: 'A screenshot showing the hidden map mode.',
      saveText:
        'The game supports saving and loading. The current game state is saved into a .sav file.',
    },
  }[language];

  return (
    <section id="jatek" className="section">
      <h2>{text.title}</h2>
      <p>{text.intro}</p>

      <ImagePlaceholder
        title={text.imageTitle}
        fileName="game-main.png"
        description={text.imageDescription}
      />

      <h3>{text.controls}</h3>
      <ul>
        <li>W - {text.up}</li>
        <li>A - {text.left}</li>
        <li>S - {text.down}</li>
        <li>D - {text.right}</li>
        <li>F5 - {text.saveKey}</li>
        <li>F9 - {text.loadKey}</li>
        <li>ESC - {text.exitKey}</li>
      </ul>

      <h3>{text.modes}</h3>
      <ul>
        <li>{text.fullMap}</li>
        <li>{text.fogMap}</li>
        <li>{text.timeLimit}</li>
      </ul>

      <ImagePlaceholder
        title={text.fogTitle}
        fileName="fog-map.png"
        description={text.fogDescription}
      />

      <h3>{text.save}</h3>
      <p>{text.saveText}</p>
    </section>
  );
}

export default GameSection;