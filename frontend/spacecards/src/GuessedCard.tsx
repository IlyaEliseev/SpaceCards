import React, { useState } from 'react';
import { Card} from 'antd';
import 'antd/dist/antd.min.css';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import {
  EditOutlined,
  CloseCircleOutlined,
} from '@ant-design/icons';
import IKnowButton from './components/IKnowButton';
import IDontKnowButton from './components/IDontKnowButton';

function GuessedCard(props: {
  key: number;
  id: number;
  frontSide: string;
  backSide: string;
  count:number
  setCount: React.Dispatch<React.SetStateAction<number>>,
  setStatistics: (cardId: Number, successValue: Number) => void
}) {
  const [size, setSize] = useState<SizeType>('large');
  const setCount = props.setCount;

  return (
    <Card
      key={props.id}
      title={props.frontSide}
      size='default'
      bordered={false}
      style={{ width: 250 }}
      hoverable={true}
      actions={[
        <IKnowButton
          setCount={setCount} 
          count={props.count} 
          cardId={props.id} 
          setStatistics={props.setStatistics}/>,
        <IDontKnowButton 
          setCount={setCount} 
          count={props.count} 
          cardId={props.id} 
          setStatistics={props.setStatistics}/>
      ]}
    >
    </Card>
  ) 
}

export default GuessedCard;