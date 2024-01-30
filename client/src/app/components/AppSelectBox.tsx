import { FormControl, InputLabel, MenuItem, Select } from "@mui/material";

interface Props {
  options: any[];
  onChange: (event: any) => void;
  selectedValue: string;
  label: string;
}

export default function AppSelectBox({
  options,
  onChange,
  selectedValue,
  label,
}: Props) {
  return (
    <FormControl fullWidth>
      <InputLabel id="select-label">{label}</InputLabel>
      <Select
        labelId="select-label"
        id="select"
        value={selectedValue}
        label={label}
        onChange={onChange}
      >
        {options.map(({ value, label }) => (
          <MenuItem value={value} key={label}>
            {label}
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
}
