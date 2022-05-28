import React from "react";
import { Card } from "antd";
import "antd/dist/antd.css";

const CardComponent: React.FC = () => (
  <div className="site-card-border-less-wrapper">
    <Card title="Card title" bordered={false} style={{ width: 300 }}>
      <p>Card content</p>
      <p>Card content</p>
      <p>Card content</p>
    </Card>
  </div>
);

export default CardComponent;
