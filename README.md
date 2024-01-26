# BarBotV2 Control Server 

This is the control software for the BarBotV2. 

This is a .NET 6 Blazor Server application setup to run on an Raspberry Pi using I2C and MariaDB/MySQL. 

## Setup 

### Hardware

The BarBot uses daisy-chained SparkFun I2C Qwiic modules to communicate with each unit, ensure that I2C is enabled on your Pi. 

Ensure that I2C SDA is connected to the host Raspberry Pi's GPIO 2 (pin 3) and SCL to GPIO 3 (pin 5) as well as one of the grounding pins, this is all the required hardware setup, WiFi/Ethernet optional. 


### Software

Install .NET 6 SDK from [dotnet.microsoft.com](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

You can quickly install .NET 6 on a Raspberry Pi (Raspbian) via: 
```
wget -O - https://raw.githubusercontent.com/pjgpetecodes/dotnet6pi/master/install.sh | sudo bash
```

Add .NET tools to your path: 
```
cat << \EOF >> ~/.bash_profile
# Add .NET Core SDK tools
export PATH="$PATH:/home/jasonalex/.dotnet/tools"
EOF
```

Or to your current session:
```
export PATH="$PATH:/home/jasonalex/.dotnet/tools"
```

Install Entity Framework tools:
```
dotnet tool install --global dotnet-ef --version 6.*
```

You can install MariaDB with:
```
sudo apt update
sudo apt upgrade
sudo apt install mariadb-server
```

#### Database 

To configure MariaDb, use:
```
sudo mysql_secure_installation
```
- When prompted for the current password, just press enter. 
- Enter 'Y' to unix_socket authentication. 
- When prompted to make a new root password, enter 'Y' and type a new root password. 
- Enter 'Y' to:
    - Remove anonymous users.
    - Disallow root login remotely. 
    - Remove the test database. 
- Finally enter 'Y' to reload the privilages. 

To administrate the db, enter: 
```
sudo mysql -uroot -p
```
And enter the root password you set before. 

### Setup the application 

- Clone & cd `[repository root]/BarBotControl`. 
- In `appsettings.json`, set `ConnectionStrings:DefaultConnection` to:
    ```
    server=localhost;user=root;password=[your set password];database=BarBotControl
    ```
- For local development, it is best to set this in dotnet user secrets via:
    ```
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "[your connection string]"
    ```

- In `appsettings.json`, set `SudoUserConfig:StartupPasscode`, this is the passcode to be used to administrate the system when there are no admin accounts (on inital startup). 
    - This can also be set in dotnet user-secrets
        ```
        dotnet user-secrets set "SudoUserConfig:StartupPasscode" "[your passcode]"
        ```

#### Setting Up The Database 
Create the database using Entity Framework: 
```
dotnet ef database update
```

## Run Debug 

You can run the application in debug mode via:
```
dotnet run
```

Then navigate to the address [localhost:5001](https://localhost:5001).

If you have Visual Studio or VS Code with the C# extension installed, you can debug the application (set breakpoints, view the watch ext.) by opening the solution from the root in Visual Studio or VS Code and pressing `F5`. 

### Configuration

In `appsettings.json`:

- `EnableConsoleDBLogging`: This flag enables the logging database information to the console, useful for debugging. 
- `EnableMSALForTesting`: This flag enables the MSAL authentication for the deployed testing instance, set this to `false` for local debugging. 
