import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import Weather from "./components/Weather";
import Second from "./components/Second";
import NotFound from "./components/NotFound";
import NavBar from "./components/NavBar";

function App() {
  return (
    <BrowserRouter>
      <NavBar />
      <Routes>
        <Route path="/" element={<Weather />} />
        <Route path="/second" element={<Second />} />
        <Route path="*" element={<NotFound />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
