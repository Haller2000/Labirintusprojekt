import { useState } from 'react';
import './App.css';

import Navbar from './components/Navbar';
import Hero from './components/Hero';
import GameSection from './components/GameSection';
import EditorSection from './components/EditorSection';
import TipsSection from './components/TipsSection';
import Footer from './components/Footer';

export type Language = 'hu' | 'en';

function App() {
  const [language, setLanguage] = useState<Language>('hu');

  return (
    <>
      <Navbar language={language} setLanguage={setLanguage} />
      <main>
        <Hero language={language} />
        <GameSection language={language} />
        <EditorSection language={language} />
        <TipsSection language={language} />
      </main>
      <Footer language={language} />
    </>
  );
}

export default App;