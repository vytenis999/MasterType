import {
  Checkbox,
  FormControl,
  InputLabel,
  ListItemText,
  MenuItem,
  OutlinedInput,
  Select,
} from "@mui/material";
import { useState } from "react";

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
  PaperProps: {
    style: {
      maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
      width: 250,
    },
  },
};

interface Props {
  items: string[];
  checked?: string[];
  onChange: (items: string[]) => void;
  label: string;
}

export default function AppMultipleSelectBox({
  items,
  checked,
  onChange,
  label,
}: Props) {
  const [checkedItems, setCheckedItems] = useState(checked || []);

  function handleChecked(event: any) {
    const {
      target: { value },
    } = event;
    let newChecked: string[] = [];
    newChecked = typeof value === "string" ? value.split(",") : value;
    setCheckedItems(newChecked);
    onChange(newChecked);
  }

  return (
    <FormControl fullWidth>
      <InputLabel id="demo-multiple-checkbox-label">{label}</InputLabel>
      <Select
        labelId="demo-multiple-checkbox-label"
        id="demo-multiple-checkbox"
        multiple
        value={checkedItems}
        onChange={handleChecked}
        input={<OutlinedInput label={label} />}
        renderValue={(selected) => selected.join(", ")}
        MenuProps={MenuProps}
      >
        {items.map((item) => (
          <MenuItem key={item} value={item}>
            <Checkbox checked={checkedItems.indexOf(item) > -1} />
            <ListItemText primary={item} />
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
}
