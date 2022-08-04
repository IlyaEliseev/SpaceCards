import { Button, Rate } from 'antd';
import React, { useState } from 'react';

function GuessingStatistics(props: { successValueList: string }) {
  const desc = ['Terrible!', 'Bad!', 'Normal!', 'Good!', 'Wonderful!'];

  const list = props.successValueList.split('');

  const correctAnswer = (value: string, index: number, thisArray: string[]) => {
    return Number(value) > 0;
  };

  const countStars = list.filter(correctAnswer).length / 2;
  const [value, setValue] = useState(countStars);

  return (
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
              <h3>{desc[value - 1]}</h3>{' '}
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
  );
}

export default GuessingStatistics;
