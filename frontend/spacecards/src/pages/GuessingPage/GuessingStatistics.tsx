import { Button, Rate } from 'antd';
import React, { useState } from 'react';
interface GuessingStatisticsProps {
  successValueList: string;
}

function GuessingStatistics(props: GuessingStatisticsProps) {
  const desc = ['Terrible!', 'Bad!', 'Normal!', 'Good!', 'Wonderful!'];
  const starsCount = desc.length;
  const list = props.successValueList.split('');

  const correctAnswer = (
    value1: string,
    index: number,
    thisArray: string[]
  ) => {
    return Number(value1) > 0;
  };

  const positiveAnswer = list.filter(correctAnswer).length;
  const stars = (starsCount * positiveAnswer) / props.successValueList.length;
  const [value, setValue] = useState(stars);

  return (
    <div className='guessingCardForm'>
      <div className='guessingStatistics'>
        <span>
          <h1>Guessing rating</h1>
          <div className='stars'>
            <Rate
              disabled
              allowHalf
              tooltips={desc}
              onChange={setValue}
              value={value}
            />
          </div>
          <div className='stars'>
            {value ? (
              <span className='ant-rate-text'>
                <h3>{desc[value - 1]}</h3>
              </span>
            ) : (
              ''
            )}
          </div>
          <div className='guessingStatisticsButton'>
            <Button type='primary' size='large' href='/guessingCards'>
              Try again
            </Button>
            <Button type='primary' size='large' href='/'>
              Back to main page
            </Button>
          </div>
        </span>
      </div>
    </div>
  );
}

export default GuessingStatistics;
