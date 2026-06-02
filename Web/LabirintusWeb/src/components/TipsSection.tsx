import type { Language } from '../App';

type TipsSectionProps = {
  language: Language;
};

function TipsSection({ language }: TipsSectionProps) {
  const text = {
    hu: {
      title: 'Tippek és tanácsok',
      cards: [
        ['Tervezd meg előre', 'Nagyobb pályák esetén érdemes előre megtervezni a főbb útvonalakat.'],
        ['Ellenőrizd a kijáratokat', 'A pálya csak akkor használható, ha legalább egy kijárattal rendelkezik.'],
        ['Ne hagyj elszigetelt elemeket', 'Az elérhetetlen járatok hibát okozhatnak a pálya betöltésekor.'],
        ['Ments gyakran', 'Hosszabb pályák készítésekor érdemes rendszeresen menteni.'],
      ],
    },
    en: {
      title: 'Tips and advice',
      cards: [
        ['Plan ahead', 'For larger maps, it is useful to plan the main routes before editing.'],
        ['Check the exits', 'A valid map should have at least one usable exit.'],
        ['Avoid isolated elements', 'Unreachable path elements can cause problems when loading the map.'],
        ['Save often', 'When creating larger maps, save your work regularly.'],
      ],
    },
  }[language];

  return (
    <section id="tippek" className="section">
      <h2>{text.title}</h2>

      <div className="tips">
        {text.cards.map(([title, description]) => (
          <div className="card" key={title}>
            <h3>{title}</h3>
            <p>{description}</p>
          </div>
        ))}
      </div>
    </section>
  );
}

export default TipsSection;