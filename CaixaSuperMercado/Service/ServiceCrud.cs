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
        string serverip = "http://127.0.0.1:5126";

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

        public async Task<T> getObjeto<T>(int? id, String name)
        {
            using (HttpClient client = new HttpClient())
            {
                // Construa a URL com o ID
                string url = $"{serverip}/api/{name}/{id}";

                // Execute a solicitação DELETE
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return System.Text.Json.JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    MessageBox.Show("Erro ao obter dados da API: " + response.StatusCode);
                    return default(T);
                }
            }
        }

        public async Task<T> salvarObjeto<T>(Object obj, string name)
        {
            try
            {
                string json = JsonConvert.SerializeObject(obj);
                using (HttpClient client = new HttpClient())
                {
                    string url = $"{serverip}/api/" + name;
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = null;
                    if (!json.Contains("\"Id\":0"))
                    {
                        response = await client.PutAsync(url, content);
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"{name} editado com sucesso!");
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            return System.Text.Json.JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                        }
                        else
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"Erro ao salvar. Código: {response.StatusCode}. Mensagem: {responseBody}. Json: {json}");
                            return default(T);
                        }
                    }
                    else
                    {
                        response = await client.PostAsync(url, content);
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            return System.Text.Json.JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                        }
                        else
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"Erro ao salvar. Código: {response.StatusCode}. Mensagem: {responseBody}. Json: {json}");
                            return default(T);
                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exceção: {ex.Message}");

                return default(T);
            }


        }
        //public async Task<List<T>> GetAll<T>()
        //{
        //    try
        //    {
        //        string url = $"{serverip}/api/" + typeof(T).Name;
        //        HttpResponseMessage response = await _httpClient.GetAsync(url);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string jsonResponse = await response.Content.ReadAsStringAsync();

        //            List<T> result = System.Text.Json.JsonSerializer.Deserialize<List<T>>(jsonResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            return result;
        //        }
        //        else
        //        {
        //            MessageBox.Show("Erro ao obter dados da API: " + response.StatusCode);
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Erro: " + ex.Message);
        //        return null;
        //    }
        //}


        public async Task<T> GetLogin<T>(string login)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"{serverip}/api/{typeof(T).Name}/login?login={login}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        return System.Text.Json.JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                    else
                    {
                        MessageBox.Show("Erro ao obter dados da API: " + response.StatusCode);
                        return default(T);
                    }
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
