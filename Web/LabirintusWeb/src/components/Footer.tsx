import type { Language } from '../App';

type FooterProps = {
  language: Language;
};

function Footer({ language }: FooterProps) {
  return (
    <footer className="footer">
      <p>
        {language === 'hu'
          ? 'Labirintus Projekt – Felhasználói dokumentáció'
          : 'Labyrinth Project – User Documentation'}
      </p>

      <p>Mechwart András Technikum</p>

      <p>2026</p>
    </footer>
  );
}

export default Footer;