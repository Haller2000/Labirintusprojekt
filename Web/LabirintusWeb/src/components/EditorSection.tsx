import type { Language } from '../App';
import ImagePlaceholder from './ImagePlaceholder';

type EditorSectionProps = {
  language: Language;
};

function EditorSection({ language }: EditorSectionProps) {
  const text = {
    hu: {
      title: 'Térképszerkesztő',
      intro:
        'A szerkesztő segítségével saját labirintusokat készíthetünk és menthetünk.',
      mainImageTitle: 'Szerkesztő főablak',
      mainImageDescription: 'A térképszerkesztő felhasználói felülete.',
      createTitle: 'Pálya létrehozása',
      createSteps: [
        'Szélesség és magasság megadása',
        'Új pálya létrehozása',
        'Elem kiválasztása',
        'Elem elhelyezése a térképen',
        'Pálya mentése',
      ],
      elementsTitle: 'Használható elemek',
      elements: [
        '█ - Kincses terem',
        '═ ║ - Egyenes járatok',
        '╔ ╗ ╚ ╝ - Sarkok',
        '╦ ╩ ╠ ╣ ╬ - Elágazások',
        '. - Üres mező',
      ],
      tilesImageTitle: 'Elemválasztó',
      tilesImageDescription: 'A használható csempék bemutatása.',
      validationTitle: 'Validálás',
      validationItems: [
        'Terem ellenőrzése',
        'Kijáratok ellenőrzése',
        'Érvényes karakterek ellenőrzése',
        'Kapcsolatok vizsgálata',
        'Elérhetetlen elemek keresése',
      ],
    },
    en: {
      title: 'Map Editor',
      intro:
        'The editor allows users to create, edit and save custom labyrinth maps.',
      mainImageTitle: 'Editor main window',
      mainImageDescription: 'The user interface of the map editor.',
      createTitle: 'Creating a map',
      createSteps: [
        'Enter the width and height',
        'Create a new map',
        'Select a tile',
        'Place the selected tile on the map',
        'Save the finished map',
      ],
      elementsTitle: 'Available elements',
      elements: [
        '█ - Treasure room',
        '═ ║ - Straight paths',
        '╔ ╗ ╚ ╝ - Corners',
        '╦ ╩ ╠ ╣ ╬ - Junctions',
        '. - Empty field',
      ],
      tilesImageTitle: 'Tile selector',
      tilesImageDescription: 'Presentation of the available tiles.',
      validationTitle: 'Validation',
      validationItems: [
        'Checking treasure rooms',
        'Checking exits',
        'Checking valid characters',
        'Checking path connections',
        'Searching for unreachable elements',
      ],
    },
  }[language];

  return (
    <section id="szerkeszto" className="section dark">
      <h2>{text.title}</h2>

      <p>{text.intro}</p>

      <ImagePlaceholder
        title={text.mainImageTitle}
        fileName="editor-main.png"
        description={text.mainImageDescription}
      />

      <h3>{text.createTitle}</h3>

      <ol>
        {text.createSteps.map((step) => (
          <li key={step}>{step}</li>
        ))}
      </ol>

      <h3>{text.elementsTitle}</h3>

      <ul>
        {text.elements.map((element) => (
          <li key={element}>{element}</li>
        ))}
      </ul>

      <ImagePlaceholder
        title={text.tilesImageTitle}
        fileName="tiles.png"
        description={text.tilesImageDescription}
      />

      <h3>{text.validationTitle}</h3>

      <ul>
        {text.validationItems.map((item) => (
          <li key={item}>{item}</li>
        ))}
      </ul>
    </section>
  );
}

export default EditorSection;