import "./App.css";

import Navbar from "./components/Navbar";
import Hero from "./components/Hero";
import GameSection from "./components/GameSection";
import EditorSection from "./components/EditorSection";
import TipsSection from "./components/TipsSection";
import Footer from "./components/Footer";

function App() {
  return (
    <>
      <Navbar />

      <Hero />

      <GameSection />

      <EditorSection />

      <TipsSection />

      <Footer />
    </>
  );
}

export default App;