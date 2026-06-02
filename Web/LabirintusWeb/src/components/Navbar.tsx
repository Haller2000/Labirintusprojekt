import type { Language } from '../App';

type NavbarProps = {
  language: Language;
  setLanguage: (language: Language) => void;
};

function Navbar({ language, setLanguage }: NavbarProps) {
  const text = {
    hu: {
      title: 'Labirintus Projekt',
      game: 'Játék',
      editor: 'Szerkesztő',
      tips: 'Tippek',
    },
    en: {
      title: 'Labyrinth Project',
      game: 'Game',
      editor: 'Editor',
      tips: 'Tips',
    },
  }[language];

  return (
    <nav className="navbar">
      <a className="logo" href="#top">
        <span className="logo-icon">╬</span>
        <span>{text.title}</span>
      </a>

      <div className="nav-right">
        <div className="nav-links">
          <a href="#jatek">{text.game}</a>
          <a href="#szerkeszto">{text.editor}</a>
          <a href="#tippek">{text.tips}</a>
        </div>

        <div className="language-switch">
          <button
            className={language === 'hu' ? 'active' : ''}
            type="button"
            onClick={() => setLanguage('hu')}
          >
            HU
          </button>
          <button
            className={language === 'en' ? 'active' : ''}
            type="button"
            onClick={() => setLanguage('en')}
          >
            EN
          </button>
        </div>
      </div>
    </nav>
  );
}

export default Navbar;