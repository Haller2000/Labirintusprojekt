import type { Language } from '../App';
import ImagePlaceholder from './ImagePlaceholder';

import jatekImg from '../../Images/jatek.png';
import jatekFedettImg from '../../Images/jatekfedett.png';

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
      imageDescription: 'A játék teljes térképes nézete.',
      fogTitle: 'Fedett térkép mód',
      fogDescription: 'A játékos csak a már bejárt részeket látja.',
      controls: 'Irányítás',
      modes: 'Játékmódok',
      save: 'Mentés és betöltés',
      saveText:
        'A játék támogatja a mentést és a visszatöltést. A játékállapot egy .sav állományba kerül mentésre.',
      items: [
        'W - Felfelé mozgás',
        'A - Balra mozgás',
        'S - Lefelé mozgás',
        'D - Jobbra mozgás',
        'F5 - Mentés',
        'F9 - Betöltés',
        'ESC - Kilépés',
      ],
      modesList: ['Teljes térkép mód', 'Fedett térkép mód', '120 másodperces időkorlát'],
    },
    en: {
      title: 'Game',
      intro:
        'The goal of the game is to discover every treasure room and then escape through an exit.',
      imageTitle: 'Game screen',
      imageDescription: 'The full map view of the game.',
      fogTitle: 'Fog of war mode',
      fogDescription: 'The player can only see the already explored parts.',
      controls: 'Controls',
      modes: 'Game modes',
      save: 'Saving and loading',
      saveText:
        'The game supports saving and loading. The current game state is saved into a .sav file.',
      items: [
        'W - Move up',
        'A - Move left',
        'S - Move down',
        'D - Move right',
        'F5 - Save',
        'F9 - Load',
        'ESC - Exit',
      ],
      modesList: ['Full map mode', 'Fog of war mode', '120 second time limit'],
    },
  }[language];

  return (
    <section id="jatek" className="section">
      <h2>{text.title}</h2>
      <p>{text.intro}</p>

      <ImagePlaceholder
        title={text.imageTitle}
        description={text.imageDescription}
        imageSrc={jatekImg}
      />

      <h3>{text.controls}</h3>
      <ul>
        {text.items.map((item) => (
          <li key={item}>{item}</li>
        ))}
      </ul>

      <h3>{text.modes}</h3>
      <ul>
        {text.modesList.map((item) => (
          <li key={item}>{item}</li>
        ))}
      </ul>

      <ImagePlaceholder
        title={text.fogTitle}
        description={text.fogDescription}
        imageSrc={jatekFedettImg}
      />

      <h3>{text.save}</h3>
      <p>{text.saveText}</p>
    </section>
  );
}

export default GameSection;