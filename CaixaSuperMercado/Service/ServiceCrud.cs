using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
namespace CaixaSuperMercado.Service
{
    public class ServiceCrud
    {
        string serverip = "http://localhost:5126";

        public async Task<bool> deletarObjeto(int? id, String name)
        {
            using (HttpClient client = new HttpClient())
            {
                // Construa a URL com o ID
                string url = $"{serverip}/api/{name}/{id}";

                // Execute a solicitação DELETE
                HttpResponseMessage response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Erro ao excluir o item.");
                    return false;
                }
            }
        }
        public async void salvarObjeto(Object obj, string name, Form frm)
        {
            try
            {
                string json = JsonConvert.SerializeObject(obj);
                using (HttpClient client = new HttpClient())
                {
                    string url = $"{serverip}/api/" + name;
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = null;
                    if (json.Contains("Id"))
                    {
                        response = await client.PutAsync(url, content);
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"{name} editado com sucesso!");
                            frm.Close();
                        }
                        else
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"Erro ao salvar. Código: {response.StatusCode}. Mensagem: {responseBody}. Json: {json}");
                        }
                    }
                    else
                    {
                        response = await client.PostAsync(url, content);
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"{name} salvo com sucesso!");
                            frm.Close();
                        }
                        else
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"Erro ao salvar. Código: {response.StatusCode}. Mensagem: {responseBody}. Json: {json}");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exceção: {ex.Message}");
            }


        }
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task<List<T>> GetAll<T>()
        {
            try
            {
                string url = $"{serverip}/api/" + typeof(T).Name;
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    List<T> result = System.Text.Json.JsonSerializer.Deserialize<List<T>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return result;
                }
                else
                {
                    MessageBox.Show("Erro ao obter dados da API: " + response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
                return null;
            }
        }


        public async Task<T> GetLogin<T>(string login)
        {
            try
            {
                string url = $"{serverip}/api/{typeof(T).Name}/login?login={login}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);


                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    T result = System.Text.Json.JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return result;
                }
                else
                {
                    MessageBox.Show("Erro ao obter dados da API: " + response.StatusCode);
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
                return default(T);
            }
        }
    }
}
