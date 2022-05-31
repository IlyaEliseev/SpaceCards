import React, { useEffect, useState } from "react";
import logo from "./logo.svg";
import "./App.css";
import CardComponent from "./components/Card";
import "antd/dist/antd.css";
import { Card } from "antd";

function App() {
  useEffect(() => {
    const fetchCards = async () => {
      const data = await fetch("https://localhost:64367/cards");
      const cards = await data.json();
      setCards(cards);
    };
    fetchCards().catch(console.error);
  });

  const [cards, setCards] = useState([]);

  const getCards = () => {
    const cardList = cards.map(
      (card: { id: number; frontSide: string; backSide: string }) => {
        return (
          <Card
            key={card.id}
            title="Space card"
            bordered={false}
            style={{ width: 300 }}
          >
            <p>{card.frontSide}</p>
            <p>{card.backSide}</p>
          </Card>
        );
      }
    );
    return cardList;
  };

  return (
    <div className="App">
      <header>
        <div className="site-card-border-less-wrapper">{getCards()}</div>
      </header>
    </div>
  );
}

export default App;
