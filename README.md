# Hospital Management System

A simple command-line-based program writtent in `C#` and `.NET 6.0` to manage a hospital system. 

## How to run the program

To run the program, it is necessary that you are using **Visual Studio IDE** on **Windows** and have `.NET 6.0` installed.

- **Clone this repository**
- **Open the solution in Visual Studio**
- **Click on the play icon to run it**

As soon as you run it, the following screen should appear:

![login-screen](./Screenshots/login-screen.jpeg)

## Login Credentials

The system only contain three types of users: Admin, Doctor, and Patient. The credentials for each user are stored in `./bin/Debug/net6.0/login-credentials.txt` and follow the formats below:

- **Admin**
  - ID: `30001`
  - Password: `admin`
- **Doctor**:
  - ID Range: `20001` - `20026` (there are 26 doctor credentials in the file)
  - All Passwords: `doc`
- **Patient**
  - ID Range: `10001` - `10026` (there are 26 patient credentials in the file)
  - All Passwords: `pat`



## User Privileges

**Note:** In addition to user-specific menu options, Admin, Doctors, and Patients share `Logout` and `Exit` features.

### Admin

Admin can view details about all doctors and patients registered in the system, as well as add new doctors and patients.

![admin-menu](./Screenshots/admin-menu.jpeg)

### Doctor

**Notes:** 

- Pre-defined doctors are listed in `./bin/Debug/net6.0/doctors.txt`
- Pre-defined patients are listed in `./bin/Debug/net6.0/patients.txt`
- Pre-defined appointments are listed in `./bin/Debug/net6.0/appointments.txt`
- Pre-defined doctor-patient relationships are listed in `./bin/Debug/net6.0/doctor-patients.txt`

Doctors can: 

1. See their personal details
2. See which patients are assigned to them (think of a GP - patient relationship)
3. See all appointments they have with all patients
4. Check the details of a particular patient
5. See all appointments they have with a given patient

![doctor-menu](./Screenshots/doctor-menu.jpeg)

### Patients

- Pre-defined doctors are listed in `./bin/Debug/net6.0/doctors.txt`
- Pre-defined patients are listed in `./bin/Debug/net6.0/patients.txt`
- Pre-defined appointments are listed in `./bin/Debug/net6.0/appointments.txt`
- Pre-defined doctor-patient relationships are listed in `./bin/Debug/net6.0/doctor-patients.txt`

Patients can:

1. See their personal details
2. See their doctor details
   - If they are not assigned to any doctors, a message will be displayed
   - If they are assigned to a doctor, the doctor details will be displayed
3. See all the appointments they have booked
4. Book an appointment with a doctor

![patient-menu](./Screenshots/patient-menu.jpeg)

## Current list of doctors

| Doctor ID | Name        | Last Name  | Email                | Phone    | Address |
| --------- | ----------- | ---------- | -------------------- | -------- | ------- |
| 20001     | Gregor      | House      | g.house@doc.com      | 04320001 |         |
| 20002     | Lisa        | Cuddy      | l.cuddy@doc.com      | 04320002 |         |
| 20003     | James       | Wilson     | j.wilson@doc.com     | 04320003 |         |
| 20004     | Eric        | Foreman    | e.foreman@doc.com    | 04320004 |         |
| 20005     | Robert      | Chase      | r.chase@doc.com      | 04320005 |         |
| 20006     | Allison     | Cameron    | a.cameron@doc.com    | 04320006 |         |
| 20007     | Remy        | Hadley     | r.hadley@doc.com     | 04320007 |         |
| 20008     | Chris       | Taub       | c.taub@doc.com       | 04320008 |         |
| 20009     | Lawrence    | Kutner     | l.kutner@doc.com     | 04320009 |         |
| 20010     | Jasseica    | Addams     | j.adams@doc.com      | 04320010 |         |
| 20011     | Chi         | Park       | c.park@doc.com       | 04320011 |         |
| 20012     | Dominika    | Petrova    | d.petrova@doc.com    | 04320012 |         |
| 20013     | Derek       | Shepherd   | d.shepherd@doc.com   | 04320013 |         |
| 20014     | Lisa        | Addison    | l.addison@doc.com    | 04320014 |         |
| 20015     | John        | Carter     | j.carter@doc.com     | 04320015 |         |
| 20016     | Christina   | Yang       | c.yang@doc.com       | 04320016 |         |
| 20017     | Meredith    | Grey       | m.grey@doc.com       | 04320017 |         |
| 20018     | Alex        | Karev      | a.karev@doc.com      | 04320018 |         |
| 20019     | Izzie       | Stevens    | i.stevens@doc.com    | 04320019 |         |
| 20020     | Miranda     | Bailey     | m.bailey@doc.com     | 04320020 |         |
| 20021     | Richard     | Webber     | r.webber@doc.com     | 04320021 |         |
| 20022     | George      | O'Malley   | g.omalley@doc.com    | 04320022 |         |
| 20023     | April       | Kepner     | a.kepner@doc.com     | 04320023 |         |
| 20024     | Amelia      | Shepherd   | a.shepherd@doc.com   | 04320024 |         |
| 20025     | Nathan      | Riggs      | n.riggs@doc.com      | 04320025 |         |
| 20026     | Harvey      | Specter    | h.specter@doc.com    | 04320026 |         |

## Current list of doctor-patient relationships

This table is to be used for reference when looking up a patients' doctor. If the field is displayed as `Not Assigned`, then it is because a patient is not assigned to a doctor yet. 

This table makes it easy to spot which doctors and patients you can manipulate in the program.

Reference file: `./bin/Debug/net6.0/doctor-patients.txt`

| Doctor ID | Doctor           | Patient ID | Patient           |
| --------- | ---------------- | ---------- | ----------------- |
| 20001     | Gregory House    | 10001      | Rebecca Adler     |
| 20002     | Lisa Cuddy       | 10002      | Clancy Harris     |
| 20005     | Robert Chase     | 10005      | John Thompson     |
| 20006     | Allison Cameron  | 10006      | Patrick Obyedkov  |
| 20007     | Remy Hadley      | 10007      | Lee Ross          |
| 20009     | Lawrence Kutner  | 10009      | Ben Taylor        |
| 20010     | Jessica Adams    | 10010      | David Young       |
| 20011     | Chi Park         | 10015      | Mara Keaton       |
| 20011     | Chi Park         | 10021      | Kyle Chandler     |
| 20012     | Dominika Petrova | 10012      | Abraham Williams  |
| 20013     | Derek Shepherd   | 10013      | Edward Mitchell   |
| 20014     | Lisa Addison     | 10004      | Eve Anderson      |
| 20014     | Lisa Addison     | 10026      | Mike Ross         |
| 20017     | Meredith Grey    | 10017      | Hannah Morgenthal |
| 20018     | Alex Karev       | 10018      | Irma Rodriguez    |
| 20019     | Izzie Stevens    | 10019      | Dylan Crandall    |
| 20022     | George O'Malley  | 10022      | Julia Carter      |
| 20023     | April Kepner     | 10023      | Patrick Clark     |
| 20025     | Nathan Riggs     | 10025      | Henry Burton      |



