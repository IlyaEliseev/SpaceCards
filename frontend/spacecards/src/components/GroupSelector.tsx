import { Select } from 'antd';
import { group } from 'console';
import React, { useEffect, useState } from 'react';

const token = sessionStorage.getItem('refreshtoken');
// 'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NjIzODY0MzUsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.CUbDEJylJxi6-m5qOfCEtqYS6SEI5mP1DOAH1GtNT_k';

function GroupSelector(props: { groups: never[]; cardId: number }) {
  const { Option } = Select;

  const [pickGroup, setPickGroup] = useState(0);
  const cardId = props.cardId;
  const groupId = pickGroup;

  const onChange = (value: { value: string; label: React.ReactNode }) => {
    setPickGroup(Number(value.value));
    localStorage.setItem(String(props.cardId), String(value.label));
  };

  const onSearch = (value: string) => {
    console.log('search:', value);
  };

  useEffect(() => {
    if (cardId > 0 && groupId > 0) {
      const addCardOnGroup = async (cardId: number, groupId: number) => {
        const data = await fetch(
          `https://localhost:49394/groups/${groupId}/cards?cardId=${cardId}`,
          {
            method: 'post',
            headers: new Headers({
              'Content-type': 'application/json',
              Authorization: `Bearer ${token}`,
            }),
          }
        );
      };
      addCardOnGroup(cardId, groupId).catch(console.error);
    }
  }, [groupId]);

  const selectGroups = () => {
    const selectGroups = props.groups.map(
      (group: { id: number; name: string }) => {
        return (
          <Option key={`${group.id}`} value={`${group.id}`} label={group.name}>
            {group.name}
          </Option>
        );
      }
    );
    return selectGroups;
  };

  return (
    <Select
      labelInValue
      showSearch
      placeholder={
        localStorage.getItem(String(props.cardId)) != null &&
        localStorage.getItem(String(props.cardId)) != 'Cards'
          ? localStorage.getItem(String(props.cardId))
          : 'Group name'
      }
      style={{ width: 120 }}
      optionFilterProp='children'
      onChange={onChange}
      onSearch={onSearch}
      filterOption={(input, option) =>
        (option!.children as unknown as string)
          .toLowerCase()
          .includes(input.toLowerCase())
      }
    >
      {selectGroups()}
    </Select>
  );
}

export default GroupSelector;
